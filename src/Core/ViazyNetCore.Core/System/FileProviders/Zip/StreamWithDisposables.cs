using System;
using System.IO;

namespace Microsoft.Extensions.FileProviders
{
    /// <summary>
    /// A <see cref="Stream"/> wrapper that holds other things that will be disposed
    /// when this is disposed.
    /// </summary>
    /// <seealso cref="System.IO.Stream" />
    sealed class StreamWithDisposables : Stream
    {
        readonly IDisposable[] _disposables;
        readonly Stream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamWithDisposables"/> class.
        /// </summary>
        /// <param name="stream">The stream to warp.</param>
        /// <param name="disposables">Other disposable objects to track.</param>
        public StreamWithDisposables(Stream stream, params IDisposable[] disposables)
        {
            this._stream = stream;
            this._disposables = disposables;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this._stream.Dispose();
                foreach (var d in this._disposables) { d.Dispose(); }
            }
        }

        public override bool CanRead => this._stream.CanRead;

        public override bool CanSeek => this._stream.CanSeek;

        public override bool CanWrite => this._stream.CanWrite;

        public override long Length => this._stream.Length;

        public override long Position { get => this._stream.Position; set => this._stream.Position = value; }

        public override void Flush() => this._stream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
            => this._stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin)
            => this._stream.Seek(offset, origin);

        public override void SetLength(long value)
            => this._stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
            => this._stream.Write(buffer, offset, count);
    }
}
