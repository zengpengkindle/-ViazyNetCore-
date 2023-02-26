using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Filters;

/// <summary>
/// 控制器异常处理
/// </summary>
public class ControllerExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ControllerExceptionFilter> _logger;

    public ControllerExceptionFilter(IWebHostEnvironment env, ILogger<ControllerExceptionFilter> logger)
    {
        _env = env;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled == false)
        {
            string message;
            var apiException = context.Exception is ApiException;
            if (_env.IsProduction())
            {
                message = apiException ? context.Exception.Message : "系统异常";
            }
            else
            {
                message = context.Exception.Message;
            }

            if (!apiException)
            {
                _logger.LogError(context.Exception, "");
            }

            var data = new ApiResponse
            {
                StatusCode = 500,
                Message = message,
            };
            context.Result = new InternalServerErrorResult(data, apiException);
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