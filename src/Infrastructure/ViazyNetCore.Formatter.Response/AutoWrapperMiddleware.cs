using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Formatter.Response
{
    internal class AutoWrapperMiddleware : WrapperBase
    {
        private readonly AutoWrapperMembers _wapperMember;
        public AutoWrapperMiddleware(RequestDelegate next, ResponseWrapperOptions options, ILogger<AutoWrapperMiddleware> logger, IActionResultExecutor<ObjectResult> executor) : base(next, options, logger, executor)
        {
            var jsonSettings = JSON.SerializerSettings;
            this._wapperMember = new AutoWrapperMembers(options, logger, jsonSettings);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await this.InvokeAsyncBase(context, this._wapperMember);
        }
    }

}
