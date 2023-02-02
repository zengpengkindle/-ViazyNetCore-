using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.DevServer
{
    /// <summary>
    /// Executes the <c>script</c> entries defined in a <c>package.json</c> file,
    /// capturing any output written to stdio.
    /// </summary>
    internal class NodeScriptRunner : IDisposable
    {
        private Process? _npmProcess;
        public EventedStreamReader StdOut { get; }
        public EventedStreamReader StdErr { get; }

        private static readonly Regex AnsiColorRegex = new("\x001b\\[[0-9;]*m", RegexOptions.None, TimeSpan.FromSeconds(1));

        public NodeScriptRunner(string workingDirectory, string scriptName, string? arguments, IDictionary<string, string?>? envVars, string pkgManagerCommand, DiagnosticSource diagnosticSource, CancellationToken applicationStoppingToken)
        {
            if (string.IsNullOrEmpty(workingDirectory))
            {
                throw new ArgumentNullException(nameof(workingDirectory));
            }

            if (string.IsNullOrEmpty(scriptName))
            {
                throw new ArgumentNullException(nameof(scriptName));
            }

            if (string.IsNullOrEmpty(pkgManagerCommand))
            {
                throw new ArgumentNullException(nameof(pkgManagerCommand));
            }

            var exeToRun = pkgManagerCommand;
            var completeArguments = $"run {scriptName} -- {arguments ?? string.Empty}";
            if (OperatingSystem.IsWindows())
            {
                // On Windows, the node executable is a .cmd file, so it can't be executed
                // directly (except with UseShellExecute=true, but that's no good, because
                // it prevents capturing stdio). So we need to invoke it via "cmd /c".
                exeToRun = "cmd";
                completeArguments = $"/c {pkgManagerCommand} {completeArguments}";
            }

            var processStartInfo = new ProcessStartInfo(exeToRun)
            {
                Arguments = completeArguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory
            };

            if (envVars != null)
            {
                foreach (var keyValuePair in envVars)
                {
                    processStartInfo.Environment[keyValuePair.Key] = keyValuePair.Value;
                }
            }

            this._npmProcess = LaunchNodeProcess(processStartInfo, pkgManagerCommand);
            this.StdOut = new EventedStreamReader(this._npmProcess.StandardOutput);
            this.StdErr = new EventedStreamReader(this._npmProcess.StandardError);

            applicationStoppingToken.Register(((IDisposable)this).Dispose);

            var ns = typeof(NodeScriptRunner).Namespace!;
            if (diagnosticSource.IsEnabled(ns))
            {
                diagnosticSource.Write(ns, new { processStartInfo, process = this._npmProcess });
            }
        }

        public NodeScriptRunner AttachToLogger(ILogger logger)
        {
            // When the node task emits complete lines, pass them through to the real logger
            this.StdOut.OnReceivedLine += line =>
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // Node tasks commonly emit ANSI colors, but it wouldn't make sense to forward
                    // those to loggers (because a logger isn't necessarily any kind of terminal)
                    LoggerMessage.Define(LogLevel.Information, new EventId(1, nameof(NodeScriptRunner)), StripAnsiColors(line))(logger, null);
                }
            };

            this.StdErr.OnReceivedLine += line =>
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    LoggerMessage.Define(LogLevel.Error, new EventId(500, nameof(NodeScriptRunner)), StripAnsiColors(line))(logger, null);
                }
            };

            // But when it emits incomplete lines, assume this is progress information and
            // hence just pass it through to StdOut regardless of logger config.
            this.StdErr.OnReceivedChunk += chunk =>
            {
                Debug.Assert(chunk.Array != null);

                var containsNewline = Array.IndexOf(chunk.Array, '\n', chunk.Offset, chunk.Count) >= 0;
                if (!containsNewline)
                {
                    Console.Write(chunk.Array, chunk.Offset, chunk.Count);
                }
            };

            return this;
        }

        private static string StripAnsiColors(string line) => AnsiColorRegex.Replace(line, string.Empty);

        private static Process LaunchNodeProcess(ProcessStartInfo startInfo, string commandName)
        {
            try
            {
                var process = Process.Start(startInfo)!;

                // See equivalent comment in OutOfProcessNodeInstance.cs for why
                process.EnableRaisingEvents = true;

                return process;
            }
            catch (Exception ex)
            {
                var message = $"Failed to start '{commandName}'. To resolve this:.\n\n"
                            + $"[1] Ensure that '{commandName}' is installed and can be found in one of the PATH directories.\n"
                            + $"    Current PATH enviroment variable is: { Environment.GetEnvironmentVariable("PATH") }\n"
                            + "    Make sure the executable is in one of those directories, or update your PATH.\n\n"
                            + "[2] See the InnerException for further details of the cause.";
                throw new InvalidOperationException(message, ex);
            }
        }

        void IDisposable.Dispose()
        {
            if (this._npmProcess != null && !this._npmProcess.HasExited)
            {
                this._npmProcess.Kill(entireProcessTree: true);
                this._npmProcess = null;
            }
        }
    }
}
