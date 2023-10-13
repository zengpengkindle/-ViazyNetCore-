using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.AspNetCore.Mvc.Filterrs
{
    [Injection(Lifetime = ServiceLifetime.Transient)]
    public class ValidationActionFilter : IAsyncActionFilter
    {
        private readonly ISerializer<string> _serializer;

        public ValidationActionFilter(ISerializer<string> serializer)
        {
            this._serializer = serializer;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction() ||
                !context.ActionDescriptor.HasObjectResult())
            {
                await next();
                return;
            }
            if (!context.ModelState.IsValid)
            {
                throw new ApiException(_serializer.Serialize(context.ModelState.Where(p => p.Value != null).SelectMany(p => p.Value!.Errors)), 400);
            }
            await next();
        }
    }
}
