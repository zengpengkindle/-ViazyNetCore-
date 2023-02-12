using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.Authorization.Modules
{
    //public class PermissionFilter : IAtionFilterAsync
    //{
    //    private readonly IAppStorage _appStorage;
    //    private readonly ITicketManager _ticketManager;
    //    private readonly PermissionService _permissionService;

    //    public PermissionFilter(PermissionService permissionService)
    //    {
    //        this._appStorage = appStorage;
    //        this._ticketManager = ticketManager;
    //        this._permissionService = permissionService;
    //    }

    //    protected override async Task ExecutingAsync(IApiContext context)
    //    {
    //        if(!context.MethodMetadata.NonAuth && this._appStorage.Identity == null)
    //        {
    //            context.Unauthorized();
    //        }
    //        else
    //        {

    //            PermissionAttribute privilegeAttr = null;
    //            var typeInfo = context.MethodMetadata.TypeMetadata.TypeInfo;
    //            object[] objAttrs = typeInfo.GetCustomAttributes(typeof(PermissionAttribute), true);

    //            var methodInfo = context.MethodMetadata.MethodInfo;
    //            var objMehtodAttrs = methodInfo.GetCustomAttributes(typeof(PermissionAttribute), true);

    //            if(objMehtodAttrs.Length > 0)
    //            {
    //                privilegeAttr = objMehtodAttrs[0] as PermissionAttribute;
    //            }
    //            else if(objAttrs.Length > 0)
    //            {
    //                privilegeAttr = objAttrs[0] as PermissionAttribute;
    //            }

    //            if(privilegeAttr != null)
    //            {
    //                //匿名访问
    //                if(privilegeAttr.PermissionKeys.Contains(PermissionIds.Anonymity))
    //                {
    //                    return;
    //                }
    //                var identity = this._appStorage.Identity;
    //                var identityId = identity.Id;
    //                var result = await this._permissionService.Check(new SimpleUser { Id = identityId, Username = identity.Username }, privilegeAttr.PermissionKeys);
    //                if(!result)
    //                {
    //                    context.Result = new StatusCodeResult(403);
    //                }
    //                //context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(Result.Fail("未授权的请求！"));

    //            }
    //        }
    //    }
    ////}
}
