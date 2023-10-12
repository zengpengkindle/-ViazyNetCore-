using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ViazyNetCore.AspNetCore.Core.Wrap;

namespace ViazyNetCore.AspNetCore.Middleware
{
    [Injection(Lifetime = ServiceLifetime.Transient)]
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IResponseWrapHandler _responseWrapHandler;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IResponseWrapHandler responseWrapHandler, IHostEnvironment environment)
        {
            this._logger = logger;
            this._responseWrapHandler = responseWrapHandler;
            this._environment = environment;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("An exception occurred, but response has already started!");
                    throw;
                }
                // TODO: 当如果不是ControllerAction 或者其他不需要拦截的异常情况，需要优化
                await this._responseWrapHandler.HandleExceptionAsync(context, ex, !_environment.IsProduction());
                //throw;
            }
        }
    }
}
