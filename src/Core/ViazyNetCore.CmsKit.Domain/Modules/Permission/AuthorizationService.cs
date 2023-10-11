using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Authorization.Modules;

namespace ViazyNetCore.CmsKit.Modules
{
    [Injection]
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IPermissionService _permissionService;
        private readonly RoleService _roleService;

        public AuthorizationService(IPermissionService permissionService, RoleService roleService)
        {
            this._permissionService = permissionService;
            this._roleService = roleService;
        }

        public async Task<bool> Check(IUser currentUser, string permissionItemKey)
        {
            if (currentUser == null)
                return false;

            if (await IsSuperAdministrator(currentUser))
                return true;
            //获取用户的所有权限
            var resolvedUserPermission = await _permissionService.ResolveUserPermission(currentUser.Id);
            if (resolvedUserPermission == null)
                return false;
            //判断用户的所有权限里有没有当前权限
            return resolvedUserPermission.Select(n => n.PermissionItemKey).Contains(permissionItemKey);
        }

        public Task<bool> IsSuperAdministrator(IUser currentUser)
        {
            return _roleService.IsUserInRoles(currentUser.Id, RoleIds.Instance().SuperAdministrator());
        }
    }
}
