using ViazyNetCore.Authorization;
using ViazyNetCore.Authorization.Models;
using ViazyNetCore.Authorization.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Dtos;
using Microsoft.AspNetCore.Authorization;
using ViazyNetCore.Auth.Authorization.ViewModels;
using ViazyNetCore.Auth.Authorization.Controllers;

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
        public async Task<bool> RemoveRole(string id)
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
        public async Task<BmsRole> FindRole(string id)
        {
            var role = await this._roleService.GetAsync(id);
            return role;
        }

        [HttpPost]
        public Task<List<string>> GetUserRole(string userId)
        {
            return this._roleService.GetRoleIdsOfUser(userId);
        }

        [HttpPost]
        public async Task<bool> UpdateUserRoles(string userId, List<string> roleIds)
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
