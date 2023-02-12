using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Filter.Extensions;

namespace ViazyNetCore.Filter.Middlewares
{
    public class ActionHandlerMiddleware
    {
        private readonly IEnumerable<IActionHandler> _handlers;
        private readonly RequestDelegate _next;

        public ActionHandlerMiddleware(IEnumerable<IActionHandler> handlers, RequestDelegate next)
        {
            this._handlers = handlers;
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null) return;
            var ahc = new ActionHandlerContext() { HttpContext = context };
            foreach (var handler in this._handlers)
            {
                await handler.ExecuteAsync(ahc);
                if (ahc.Result != null)
                {
                    await ahc.Result.ExecuteResultAsync(context);
                    return;
                }
            }
            await this._next(context);
        }
    }
}
