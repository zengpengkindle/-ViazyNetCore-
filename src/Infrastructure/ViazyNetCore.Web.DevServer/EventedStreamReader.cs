using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.DevServer
{
    /// <summary>
    /// Wraps a <see cref="StreamReader"/> to expose an evented API, issuing notifications
    /// when the stream emits partial lines, completed lines, or finally closes.
    /// </summary>
    class EventedStreamReader
    {
        public delegate void OnReceivedChunkHandler(ArraySegment<char> chunk);
        public delegate void OnReceivedLineHandler(string line);
        public delegate void OnStreamClosedHandler();

        public event OnReceivedChunkHandler? OnReceivedChunk;
        public event OnReceivedLineHandler? OnReceivedLine;
        public event OnStreamClosedHandler? OnStreamClosed;

        private readonly StreamReader _streamReader;
        private readonly StringBuilder _linesBuffer;

        public EventedStreamReader(StreamReader streamReader)
        {
            this._streamReader = streamReader ?? throw new ArgumentNullException(nameof(streamReader));
            this._linesBuffer = new StringBuilder();
            Task.Factory.StartNew(this.Run, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        public Task<Match> WaitForMatch(Regex regex)
        {
            var tcs = new TaskCompletionSource<Match>();
            var completionLock = new object();

            OnReceivedLineHandler? onReceivedLineHandler = null;
            OnStreamClosedHandler? onStreamClosedHandler = null;

            void ResolveIfStillPending(Action applyResolution)
            {
                lock (completionLock)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        OnReceivedLine -= onReceivedLineHandler;
                        OnStreamClosed -= onStreamClosedHandler;
                        applyResolution();
                    }
                }
            }

            onReceivedLineHandler = line =>
            {
                var match = regex.Match(line);
                if (match.Success)
                {
                    ResolveIfStillPending(() => tcs.SetResult(match));
                }
            };

            onStreamClosedHandler = () =>
            {
                ResolveIfStillPending(() => tcs.SetException(new EndOfStreamException()));
            };

            OnReceivedLine += onReceivedLineHandler;
            OnStreamClosed += onStreamClosedHandler;

            return tcs.Task;
        }

        private async Task Run()
        {
            var buf = new char[8 * 1024];
            while (true)
            {
                var chunkLength = await this._streamReader.ReadAsync(buf, 0, buf.Length);
                if (chunkLength == 0)
                {
                    if (this._linesBuffer.Length > 0)
                    {
                        this.OnCompleteLine(this._linesBuffer.ToString());
                        this._linesBuffer.Clear();
                    }

                    this.OnClosed();
                    break;
                }

                this.OnChunk(new ArraySegment<char>(buf, 0, chunkLength));

                int startPos = 0;
                int lineBreakPos;

                // get all the newlines
                while ((lineBreakPos = Array.IndexOf(buf, '\n', startPos, chunkLength - startPos)) >= 0 && startPos < chunkLength)
                {
                    var length = (lineBreakPos + 1) - startPos;
                    this._linesBuffer.Append(buf, startPos, length);
                    this.OnCompleteLine(this._linesBuffer.ToString());
                    this._linesBuffer.Clear();
                    startPos = lineBreakPos + 1;
                }

                // get the rest
                if (lineBreakPos < 0 && startPos < chunkLength)
                {
                    this._linesBuffer.Append(buf, startPos, chunkLength - startPos);
                }
            }
        }

        private void OnChunk(ArraySegment<char> chunk)
        {
            var dlg = OnReceivedChunk;
            dlg?.Invoke(chunk);
        }

        private void OnCompleteLine(string line)
        {
            var dlg = OnReceivedLine;
            dlg?.Invoke(line);
        }

        private void OnClosed()
        {
            var dlg = OnStreamClosed;
            dlg?.Invoke();
        }
    }
}
