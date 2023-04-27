using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Auth.Authorization.ViewModels;
using ViazyNetCore.Authorization.Modules;

namespace ViazyNetCore.Authrozation
{
    [Permission(PermissionIds.User)]
    public class RoleController : DynamicControllerBase
    {
        private readonly RoleService _roleService;
        private readonly PermissionService _permissionService;

        public RoleController(RoleService roleService, PermissionService permissionService)
        {
            this._roleService = roleService;
            this._permissionService = permissionService;
        }

        [HttpPost]
        public Task<PageData<RoleFindAllModel>> FindRoles(FindRolesParameters args)
        {
            return this._roleService.FindRoles(args);
        }

        [HttpPost]
        public Task<List<RoleSimpleModel>> GetAll()
        {
            return this._roleService.GetAllAsync();
        }

        [HttpPost]
        public async Task<bool> RemoveRole(long id)
        {
            if (id == RoleIds.Instance().SuperAdministrator())
            {
                throw new ApiException("超级管理员禁止删除");
            }
            await this._roleService.DeleteRoleAsync(id);
            await _permissionService.UpdatePermissionsInUserRole(null, id, OwnerType.Role);
            return true;
            //await context.Table<BmsPermission>().Where(w => w.OwnerId == id && w.OwnerType == OwnerType.Role).RemoveAsync();

        }

        [HttpPost]
        public async Task<bool> UpdateRole(UpdateBmsRoleArgs args)
        {
            var role = args.CopyTo<BmsRole>();
            await this._roleService.UpdateAsync(role);
            //await _permissionService.UpdatePermissionsInUserRole(args.Keys, args.Id, OwnerType.Role);
            return true;
        }

        [HttpPost]
        public async Task<BmsRole> FindRole(long id)
        {
            var role = await this._roleService.GetAsync(id);
            return role;
        }

        [HttpPost]
        public Task<List<long>> GetUserRoleIds(long userId)
        {
            return this._roleService.GetRoleIdsOfUser(userId);
        }

        [HttpPost]
        public async Task<bool> UpdateUserRoles(long userId, List<long> roleIds)
        {
            await this._roleService.UpdateUserToRoles(userId, roleIds);
            return true;
        }
    }

    public class FindRolesParameters : Pagination
    {
        public string? NameLike { get; set; }
    }
}
