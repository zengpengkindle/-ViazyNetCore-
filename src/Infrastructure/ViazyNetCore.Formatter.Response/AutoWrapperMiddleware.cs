using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Formatter.Response
{
    internal class AutoWrapperMiddleware : WrapperBase
    {
        private readonly AutoWrapperMembers _awm;
        public AutoWrapperMiddleware(RequestDelegate next, ResponseWrapperOptions options, ILogger<AutoWrapperMiddleware> logger, IActionResultExecutor<ObjectResult> executor) : base(next, options, logger, executor)
        {
            var jsonSettings = Helpers.JSONHelper.GetJSONSettings();
            _awm = new AutoWrapperMembers(options, logger, jsonSettings);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await InvokeAsyncBase(context, _awm);
        }
    }

}
