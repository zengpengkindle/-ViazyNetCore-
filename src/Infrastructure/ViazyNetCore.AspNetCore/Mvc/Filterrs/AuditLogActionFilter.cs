using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ViazyNetCore.AspNetCore.Core.Auditing;

namespace ViazyNetCore.AspNetCore.Mvc.Filterrs
{
    [Injection(Lifetime = ServiceLifetime.Transient)]
    public class AuditLogActionFilter : IAsyncActionFilter
    {
        private readonly ActionFilterOptions _options;
        private readonly ILogger<AuditLogActionFilter> _logger;

        public AuditLogActionFilter(IOptions<ActionFilterOptions> options, ILogger<AuditLogActionFilter> logger)
        {
            this._options = options.Value;
            this._logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!ShouldSaveAudit(context, out var auditLogInfo))
            {
                await next();
                return;
            }
            bool isRequestSuccess = false;
            string? responseBody = null;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                using var memoryStream = new MemoryStream();

                context.HttpContext.Response.Body = memoryStream;
                var result = await next();
                if (result.Exception != null && !result.ExceptionHandled)
                {
                    auditLogInfo.Exceptions.Add(result.Exception);
                    isRequestSuccess = false;
                }
                responseBody = await ReadResponseBodyStreamAsync(memoryStream);
                return;
            }
            catch (Exception ex)
            {
                isRequestSuccess = false;
                auditLogInfo.Exceptions.Add(ex);
                throw;
            }
            finally
            {
                auditLogInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                stopwatch.Stop();
                if (context.HttpContext.Response.HasStarted)
                {
                    this.LogResponseHasStartedError();
                }
                else
                {
                    this.LogHttpRequest(auditLogInfo, responseBody, isRequestSuccess);
                }
            }
        }

        public bool ShouldSaveAudit(ActionExecutingContext context, out AuditLogInfo auditLog)
        {
            auditLog = null;
            if (!this._options.EnableResponseLogging)
            {
                return false;
            }

            if (context.HttpContext.WebSockets.IsWebSocketRequest)
            {
                return false;
            }

            if (!context.ActionDescriptor.IsControllerAction())
            {
                return false;
            }

            var auditingHelper = context.GetRequiredService<IAuditingHelper>();
            if (auditingHelper == null)
            {
                return false;
            }

            if (!auditingHelper.ShouldSaveAudit(
                    context.ActionDescriptor.GetMethodInfo(),
                    defaultValue: this._options.EnableResponseLogging))
            {
                return false;
            }

            auditLog = auditingHelper.CreateAuditLogInfo();
            PreContribute(context.HttpContext, auditLog);
            return true;
        }

        public void PreContribute(HttpContext httpContext, AuditLogInfo auditLog)
        {
            if (httpContext == null)
            {
                return;
            }

            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            if (auditLog.HttpMethod == null)
            {
                auditLog.HttpMethod = httpContext.Request.Method;
            }

            if (auditLog.Url == null)
            {
                auditLog.Url = BuildUrl(httpContext);
            }

            if (auditLog.ClientIpAddress == null)
            {
                auditLog.ClientIpAddress = httpContext.GetRequestIP();
            }

            if (auditLog.BrowserInfo == null)
            {
                auditLog.BrowserInfo = httpContext.Request?.Headers?["User-Agent"];
            }
            if (auditLog.BrowserInfo == null)
            {
                auditLog.Url = BuildUrl(httpContext);
            }
            if (auditLog.BrowserInfo == null)
            {
                auditLog.CorrelationId = GetCorrelationId(httpContext);
            }
            //TODO: context.AuditInfo.ClientName
        }


        protected virtual string BuildUrl(HttpContext httpContext)
        {
            //TODO: Add options to include/exclude query, schema and host

            var uriBuilder = new UriBuilder
            {
                Scheme = httpContext.Request.Scheme,
                Host = httpContext.Request.Host.Host,
                Path = httpContext.Request.Path.ToString(),
                Query = httpContext.Request.QueryString.ToString()
            };

            return uriBuilder.Uri.AbsolutePath;
        }

        public virtual string GetCorrelationId(HttpContext httpContext)
        {
            if (httpContext?.Request?.Headers == null)
            {
                return CreateNewCorrelationId();
            }

            string correlationId = httpContext.Request.Headers[this._options.HttpHeaderLogIdName];

            if (correlationId.IsNull())
            {
                lock (httpContext.Request.Headers)
                {
                    if (correlationId.IsNull())
                    {
                        correlationId = CreateNewCorrelationId();
                        httpContext.Request.Headers[this._options.HttpHeaderLogIdName] = correlationId;
                    }
                }
            }

            return correlationId;
        }

        protected virtual string CreateNewCorrelationId()
        {
            return Guid.NewGuid().ToString("N");
        }

        private void LogResponseHasStartedError()
        {
            _logger.Log(LogLevel.Warning, "The response has already started, the AutoWrapper middleware will not be executed.");
        }

        private void LogHttpRequest(AuditLogInfo auditLogInfo, string? reponseBody, bool isRequestSuccess)
        {
            var message = auditLogInfo.ToString();
            if (this._options.EnableResponseLogging)
            {
                message += $"\r\nResponse: {reponseBody} ";
            }
            _logger.Log(isRequestSuccess ? LogLevel.Information : LogLevel.Error, message);
        }

        public async Task<string> ReadResponseBodyStreamAsync(Stream bodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(bodyStream).ReadToEndAsync();
            bodyStream.Seek(0, SeekOrigin.Begin);

            return responseBody;
        }
    }
}
