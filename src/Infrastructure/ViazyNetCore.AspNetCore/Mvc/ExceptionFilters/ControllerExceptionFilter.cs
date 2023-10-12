using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ViazyNetCore.AspNetCore.Core.Wrap;

namespace ViazyNetCore.AspNetCore.Mvc.ExceptionFilters;

/// <summary>
/// 控制器异常处理
/// </summary>
[Injection(Lifetime = ServiceLifetime.Transient)]
public class ControllerExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ControllerExceptionFilter> _logger;
    private readonly IResponseWrapHandler _responseWrapHandler;

    public ControllerExceptionFilter(IWebHostEnvironment env, ILogger<ControllerExceptionFilter> logger, IResponseWrapHandler responseWrapHandler)
    {
        _env = env;
        _logger = logger;
        this._responseWrapHandler = responseWrapHandler;
    }

    public void OnException(ExceptionContext context)
    {
        if (!context.ExceptionHandled)
        {
            this._responseWrapHandler.HandleExceptionAsync(context.HttpContext, context.Exception, !_env.IsProduction());
        }

        context.ExceptionHandled = true;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        return Task.CompletedTask;
    }
}

public class InternalServerErrorResult : ObjectResult
{
    public InternalServerErrorResult(object value, bool appException) : base(value)
    {
        StatusCode = appException ? Microsoft.AspNetCore.Http.StatusCodes.Status200OK : Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
    }
}