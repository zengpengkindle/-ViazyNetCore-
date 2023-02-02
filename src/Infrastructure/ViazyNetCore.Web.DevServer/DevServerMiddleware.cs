using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.DevServer
{
    class DevServerMiddleware
    {
        private const string LogCategoryName = nameof(DevServerMiddleware);
        private static readonly TimeSpan RegexMatchTimeout = TimeSpan.FromSeconds(5);

        public static void Attach(ISpaBuilder spaBuilder, NodeServerOptionsBase options)
        {
            var pkgManagerCommand = spaBuilder.Options.PackageManagerCommand;
            var sourcePath = spaBuilder.Options.SourcePath!;
            if (options.Port == 0) options.Port = spaBuilder.Options.DevServerPort;
            var appBuilder = spaBuilder.ApplicationBuilder;
            var applicationStoppingToken = appBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;
            var logger = LoggerFinder.GetOrCreateLogger(appBuilder, LogCategoryName);
            var diagnosticSource = appBuilder.ApplicationServices.GetRequiredService<DiagnosticSource>();

            var portTask = StartNodeServerAsync(sourcePath, options, pkgManagerCommand, logger, diagnosticSource, applicationStoppingToken);

            var targetUriTask = portTask.ContinueWith(task => new UriBuilder(options.Scheme, options.Host, task.Result).Uri);

            spaBuilder.UseProxyToSpaDevelopmentServer(() =>
            {
                var timeout = spaBuilder.Options.StartupTimeout;
                return targetUriTask.WithTimeout(timeout, "The dev server did not start listening for requests " +
                                                          $"within the timeout period of {timeout.TotalSeconds} seconds. " +
                                                          "Check the log output for error information.");
            });
        }

        private static async Task<int> StartNodeServerAsync(string sourcePath, NodeServerOptionsBase options, string pkgManagerCommand, ILogger logger, DiagnosticSource diagnosticSource, CancellationToken applicationStoppingToken)
        {
            if (options.Port == 0)
            {
                options.Port = TcpPortFinder.FindAvailablePort();
            }
            //var configuration = spaBuilder.ApplicationBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
            //var nodeEnv = options.Environment is null ? new() : new Dictionary<string, string?>(options.Environment);

            //var port = configuration["ASPNETCORE_URLS"].Split(';').Select(u => new Uri(u)).Where(u => u.Scheme == "https").Select(u => u.Port).First();
            //nodeEnv.Add("HRM_PORT", port.ToString());
            //logger.LogInformation("Starting dev server on port {portNumber}...", options.Port);

            var scriptRunner = new NodeScriptRunner(sourcePath, options.ScriptName, options.GenerateArguments(), options.Environment, pkgManagerCommand, diagnosticSource, applicationStoppingToken)
                .AttachToLogger(logger);

            using (var stdErrReader = new EventedStreamStringReader(scriptRunner.StdOut))
            {
                try
                {
                    // TODO vite 3.0 后启动输出为 VITE {vesion} ready in {time}ms
                    //await scriptRunner.StdOut.WaitForMatch(new Regex("dev server running at", RegexOptions.None, RegexMatchTimeout));
                    await scriptRunner.StdOut.WaitForMatch(new Regex(@"(ready in)|(dev server running at)", RegexOptions.None, RegexMatchTimeout));
                }
                catch (EndOfStreamException ex)
                {
                    throw new InvalidOperationException(
                        $"The NPM script '{options.ScriptName}' exited without indicating that the " +
                        $"dev server was listening for requests. The error output was: " +
                        $"{stdErrReader.ReadAsString()}", ex);
                }
            }

            return options.Port;
        }

    }
}
