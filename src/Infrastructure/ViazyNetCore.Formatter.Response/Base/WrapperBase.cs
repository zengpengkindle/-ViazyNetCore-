using ViazyNetCore.Formatter.Response.Extensions;
using ViazyNetCore.Formatter.Response.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ViazyNetCore.Formatter.Response.Filters;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace ViazyNetCore.Formatter.Response
{
    internal abstract class WrapperBase
    {
        private readonly RequestDelegate _next;
        private readonly ResponseWrapperOptions _options;
        private readonly ILogger<AutoWrapperMiddleware> _logger;
        private IActionResultExecutor<ObjectResult> _executor { get; }
        public WrapperBase(RequestDelegate next, ResponseWrapperOptions options, ILogger<AutoWrapperMiddleware> logger, IActionResultExecutor<ObjectResult> executor)
        {
            _next = next;
            _options = options;
            _logger = logger;
            _executor = executor;
        }

        public virtual async Task InvokeAsyncBase(HttpContext context, AutoWrapperMembers awm)
        {
            if (awm.IsSwagger(context) || !awm.IsApi(context))
                await _next(context);
            else if (context.WebSockets.IsWebSocketRequest || context.Request.Path.StartsWithSegments("/messageHub", StringComparison.CurrentCultureIgnoreCase))
            {
                await _next(context);
            }
            else
            {
                var stopWatch = Stopwatch.StartNew();
                var requestBody = await awm.GetRequestBodyAsync(context.Request);
                var originalResponseBodyStream = context.Response.Body;
                bool isRequestSuccess = false;

                using var memoryStream = new MemoryStream();
                string? responseBody = null;
                try
                {
                    context.Response.Body = memoryStream;
                    await _next.Invoke(context);
                    if (context.Response.HasStarted) { LogResponseHasStartedError(); return; }

                    var endpoint = context.GetEndpoint();
                    if (endpoint?.Metadata?.GetMetadata<AutoWrapIgnoreAttribute>() is object)
                    {
                        await awm.RevertResponseBodyStreamAsync(memoryStream, originalResponseBodyStream);
                        return;
                    }

                    responseBody = await awm.ReadResponseBodyStreamAsync(memoryStream);
                    context.Response.Body = originalResponseBodyStream;

                    if (context.Response.StatusCode != Status304NotModified && context.Response.StatusCode != Status204NoContent)
                    {

                        if (!_options.IsApiOnly && (responseBody.IsHtml() && !_options.BypassHTMLValidation) && context.Response.StatusCode == Status200OK)
                        {
                            context.Response.StatusCode = Status404NotFound;
                        }

                        if (!context.Request.Path.StartsWithSegments(new PathString(_options.WrapWhenApiPathStartsWith))
                            && (responseBody.IsHtml()
                            && !_options.BypassHTMLValidation)
                            && context.Response.StatusCode == Status200OK)
                        {
                            if (memoryStream.Length > 0) { await awm.HandleNotApiRequestAsync(context); }
                            return;
                        }

                        isRequestSuccess = awm.IsRequestSuccessful(context.Response.StatusCode);
                        if (isRequestSuccess)
                        {
                            await awm.HandleSuccessfulRequestAsync(context, responseBody);
                        }
                        else
                        {
                            await awm.HandleUnsuccessfulRequestAsync(context, responseBody, context.Response.StatusCode);
                        }
                    }

                }
                catch (Exception exception)
                {
                    if (context.Response.HasStarted) { LogResponseHasStartedError(); return; }
                    await awm.HandleExceptionAsync(context, exception);
                    await awm.RevertResponseBodyStreamAsync(memoryStream, originalResponseBodyStream);
                }
                finally
                {
                    LogHttpRequest(context, requestBody, responseBody, stopWatch, isRequestSuccess);
                }
            }
        }

        private bool ShouldLogRequestData(HttpContext context)
        {
            if (_options.ShouldLogRequestData)
            {
                var endpoint = context.GetEndpoint();
                return (endpoint?.Metadata?.GetMetadata<RequestDataLogIgnoreAttribute>() is null);
            }

            return false;
        }

        private void LogHttpRequest(HttpContext context, string requestBody, string? reponseBody, Stopwatch stopWatch, bool isRequestOk)
        {
            stopWatch.Stop();
            if (this._options.EnableResponseLogging)
            {
                bool shouldLogRequestData = ShouldLogRequestData(context);

                var request = shouldLogRequestData
                            ? isRequestOk
                                ? $"{context.Request.Method} {context.Request.Scheme} {context.Request.Host}{context.Request.Path} {context.Request.QueryString} {requestBody}"
                                : (!isRequestOk && this._options.LogRequestDataOnException)
                                   ? $"{context.Request.Method} {context.Request.Scheme} {context.Request.Host}{context.Request.Path} {context.Request.QueryString} {requestBody}"
                                   : $"{context.Request.Method} {context.Request.Scheme} {context.Request.Host}{context.Request.Path}"
                            : $"{context.Request.Method} {context.Request.Scheme} {context.Request.Host}{context.Request.Path}";

                _logger.Log(LogLevel.Information, $"Source:[{context.Connection.RemoteIpAddress}] " +
                                                  $"Request: {request} " +
                                                  $"Responded with [{context.Response.StatusCode}] in {stopWatch.ElapsedMilliseconds}ms"
                                                 + $"Response: {reponseBody} ");
            }
        }

        private void LogResponseHasStartedError()
        {
            _logger.Log(LogLevel.Warning, "The response has already started, the AutoWrapper middleware will not be executed.");
        }
    }

}
