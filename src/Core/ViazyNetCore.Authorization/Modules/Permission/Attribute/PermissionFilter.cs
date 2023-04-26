using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ViazyNetCore.Authorization.Modules
{
    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly IPermissionService _permissionService;

        public PermissionFilter(IPermissionService permissionService)
        {
            this._permissionService = permissionService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User?.Identity?.IsAuthenticated == true)
            {
                await this.ExecutingAsync(context);
            }
        }

        protected async Task ExecutingAsync(AuthorizationFilterContext context)
        {
            PermissionAttribute? privilegeAttr = null;

            var methodInfo = context.ActionDescriptor as ControllerActionDescriptor;
            if (methodInfo == null)
                return;
            var objControllerAttrs = methodInfo.ControllerTypeInfo.GetCustomAttributes(typeof(PermissionAttribute), true);
            var objMehtodAttrs = methodInfo.MethodInfo.GetCustomAttributes(typeof(PermissionAttribute), true);

            if (objMehtodAttrs.Length > 0)
            {
                privilegeAttr = objMehtodAttrs[0] as PermissionAttribute;
            }
            else if (objControllerAttrs.Length > 0)
            {
                privilegeAttr = objControllerAttrs[0] as PermissionAttribute;
            }

            if (privilegeAttr != null)
            {
                //匿名访问
                if (privilegeAttr.PermissionKeys.Contains(PermissionIds.Anonymity))
                {
                    return;
                }
                var result = await this._permissionService.Check(context.HttpContext.GetAuthUser()
                    , privilegeAttr?.PermissionKeys ?? Array.Empty<string>());
                if (!result)
                {
                    throw new ApiException(403);
                }
                //context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(Result.Fail("未授权的请求！"));
            }
        }
    }
}
