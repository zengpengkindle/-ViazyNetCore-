using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ViazyNetCore.Filter.Extensions
{
    internal static class WebExtensions
    {
        private readonly static Microsoft.AspNetCore.Routing.RouteData EmptyRouteData = new Microsoft.AspNetCore.Routing.RouteData();
        private readonly static Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor EmptyActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();

        /// <summary>
        /// 执行操作结果。
        /// </summary>
        /// <param name="result">结果。</param>
        /// <param name="context">HTTP 上下文。</param>
        /// <returns>异步任务。</returns>
        public static Task ExecuteResultAsync(this Microsoft.AspNetCore.Mvc.IActionResult result, HttpContext context)
        {
            return result.ExecuteResultAsync(new Microsoft.AspNetCore.Mvc.ActionContext(context, EmptyRouteData, EmptyActionDescriptor));
        }
    }
}
