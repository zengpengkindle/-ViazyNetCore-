using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViazyNetCore.AspNetCore.Core.Wrap;

namespace ViazyNetCore.AspNetCore.Mvc.Filterrs
{
    [Injection(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
    public class ResponseWrapActionFilter : IAsyncActionFilter
    {
        public ResponseWrapActionFilter()
        {

        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionExecutedContext = await next();

            if (context.HttpContext.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            if (context.ActionDescriptor.EndpointMetadata.Any(m => m is AutoWrapIgnoreAttribute))
            {
                return;
            }

            IActionResult result = actionExecutedContext.Result;

            var formatResult = result switch
            {
                ViewResult => false,
                PartialViewResult => false,
                ViewComponentResult => false,
                PageResult => false,
                FileResult => false,
                SignInResult => false,
                SignOutResult => false,
                RedirectToPageResult => false,
                RedirectToRouteResult => false,
                RedirectResult => false,
                RedirectToActionResult => false,
                LocalRedirectResult => false,
                ChallengeResult => false,
                ForbidResult => false,
                BadRequestObjectResult => false,
                _ => true,
            };

            if (!formatResult)
            {
                return;
            }

            var data = result switch
            {
                ContentResult contentResult => contentResult.Content,
                ObjectResult objectResult => objectResult.Value,
                JsonResult jsonResult => jsonResult.Value,
                _ => null,
            };

            if (data is ApiResponse)
            {
                return;
            }

            actionExecutedContext.Result = new JsonResult(new ApiResponse
            {
                StatusCode = 200,
                Message = $"{context.HttpContext.Request.Method} {ResponseMessage.Success}",
                Data = new WrapperData
                {
                    Success = true,
                    Result = data
                },
            });
        }
    }
}
