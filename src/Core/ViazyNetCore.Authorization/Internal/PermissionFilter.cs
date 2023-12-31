﻿using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ViazyNetCore.Authorization.Modules;

namespace ViazyNetCore.Authorization
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
            var objControllerAttr = methodInfo.ControllerTypeInfo.GetCustomAttribute(typeof(PermissionAttribute), true);
            var objMehtodAttr = methodInfo.MethodInfo.GetCustomAttribute(typeof(PermissionAttribute), true);

            if (objMehtodAttr != null)
            {
                privilegeAttr = objMehtodAttr as PermissionAttribute;
            }
            else if (objControllerAttr != null)
            {
                privilegeAttr = objControllerAttr as PermissionAttribute;
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
            else
            {
                // 当前 PermissionAttribute 不与 ApiPermissionAttrubite 共存
                var apiPermissionMehtodAttr = methodInfo.MethodInfo.GetCustomAttribute(typeof(ApiPermissionAttribute), false);
                if (apiPermissionMehtodAttr != null)
                {

                    var action = context.ActionDescriptor as ControllerActionDescriptor;
                    var routeTemplate = action?.AttributeRouteInfo?.Template;

                    var result = await this._permissionService.CheckApi(context.HttpContext.GetAuthUser(),
                           routeTemplate, context.HttpContext.Request.Method);
                    if (!result)
                    {
                        throw new ApiException(403);
                    }
                }
            }
        }
    }
}
