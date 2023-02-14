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

namespace ViazyNetCore.Authrozation
{
    [Authorize]
    [ApiController]
    [Permission(PermissionIds.User)]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly PermissionService _permissionService;

        public RoleController(RoleService roleService, PermissionService permissionService)
        {
            this._roleService = roleService;
            this._permissionService = permissionService;
        }

        [HttpPost, Route("findRoles")]
        public Task<PageData<RoleFindAllModel>> FindRoles(FindRolesParameters args)
        {
            return this._roleService.FindRoles(args);
        }

        [HttpPost, Route("getAll")]
        public Task<List<RoleSimpleModel>> GetAll()
        {
            return this._roleService.GetAllAsync();
        }

        [HttpPost, Route("removeRole")]
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

        [HttpPost, Route("updateRole")]
        public async Task<bool> UpdateRole(UpdateBmsRoleArgs args)
        {
            //var query = this._engine.Table<BmsRole>();
            //if (await query.AnyAsync(r => r.Name == item.Name && r.Id != item.Id)) throw new ApiException($"角色名称【{item.Name}】已存在！");

            //if (item.Id.IsNull())
            //{
            //    item.Id = Snowflake.NextIdString();
            //    item.Status = ComStatus.Enabled;
            //    await query.AddAsync(item);
            //}
            //else
            //{
            //    await query.ModifyAsync(item);
            //}
            var role = args.CopyTo<BmsRole>();
            await this._roleService.UpdateAsync(role);
            await _permissionService.UpdatePermissionsInUserRole(args.Keys, args.Id, OwnerType.Role);
            return true;
        }

        [HttpPost, Route("findRole")]
        public async Task<BmsRole> FindRole(string id)
        {
            var role = await this._roleService.GetAsync(id);
            return role;
        }

    }

    public class FindRolesParameters : Pagination
    {
        public string NameLike { get; set; }
    }
}
