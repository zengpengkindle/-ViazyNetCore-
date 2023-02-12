using ViazyNetCore.Authorization;
using ViazyNetCore.Authorization.Models;
using ViazyNetCore.Authorization.Modules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authrozation
{
    [Permission(PermissionIds.User)]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly PermissionService _permissionService;

        public PermissionController(RoleService roleService, PermissionService permissionService)
        {
            this._roleService = roleService;
            this._permissionService = permissionService;
        }

        public async Task<List<PermissionKey>> GetAll()
        {
            var result = await this._permissionService.GetAll();

            return result;
        }

        public async Task<List<string>> GetRolePermission(string roleId)
        {
            var result = await this._permissionService.GetPermissionsInUserRole(roleId, OwnerType.Role);

            return result.Select(p => p.PermissionItemKey).ToList();
        }

        public async Task UpdatePermissionsInRole(string roleId, string[] key)
        {
            await _permissionService.UpdatePermissionsInUserRole(key, roleId, OwnerType.Role);
        }

        public async Task<int> AddPermission(PermissionModel permissionModel)
        {
            await this._permissionService.AddPermission(permissionModel.Name, permissionModel.Key);

            var permission = await this._permissionService.GetPermissionByPermissionKey(permissionModel.Key);
            return permission.Id;
        }

        public async Task<List<string>> GetMenuKeysInPermissionKey(string permissionKey)
        {
            var result = await this._permissionService.GetMenusInPermissionKey(permissionKey);

            return result.Select(p => p.Id).ToList();
        }
        public async Task UpdateMenusInPermission(string permissionKey, string[] menuIds)
        {
            await this._permissionService.UpdateMenusInPermission(permissionKey, menuIds);
        }


        #region 功能菜单管理
        public async Task<MenuTreeModel> GetMenus()
        {
            var result = await this._permissionService.GetAllMenu();
            var trees = new List<MenuTreeModel>();

            var tree = new MenuTreeModel
            {
                Id = "10000",
                Label = "菜单管理",
                Type = MenuType.MidNode,
                Icon = "el-icon-tickets"
            };
            BindTree(tree, result);
            trees.Add(tree);
            return tree;
        }

        public async Task<BmsMenus> GetMenu(string id)
        {
            var result = await this._permissionService.GetMenu(id);
            return result;
        }

        private void BindTree(MenuTreeModel treeModel, List<BmsMenus> source)
        {
            var menus = source.Where(p => p.ParentId == treeModel.Id).OrderBy(p => p.OrderId);
            if(menus.Count() == 0)
                return;
            treeModel.Children = new List<MenuTreeModel>();
            foreach(var menu in menus)
            {
                var tree = new MenuTreeModel
                {
                    Id = menu.Id,
                    Label = menu.Name,
                    Type = menu.Type,
                    Icon = menu.Icon
                };
                BindTree(tree, source);
                treeModel.Children.Add(tree);
            }
        }

        public Task UpdateMenu(BmsMenus item)
        {
            return this._permissionService.UpdateMenus(item);
        }

        public Task RemoveMenu(string menuId) {
            return this._permissionService.RemoveMenu(menuId);
        }

        #endregion

        #region 菜单

        [Permission(PermissionIds.Anonymity)]
        public async Task<List<PrivTreeModel>> GetUserMenus()
        {
            var auth = this.HttpContext.GetAuthUser();
            var privs = await this._permissionService.ResolveUserPermission(auth.UserKey);
            var privKeys = privs.Select(p => p.PermissionItemKey).Distinct().ToArray();
            if(auth.UserName == "admin")
            {
                if(!privKeys.Contains(PermissionIds.User))
                    privKeys = privKeys.Concat(new string[] { PermissionIds.User }).ToArray();
            }
            var menus = await this._permissionService.GetMenusInPermissionKeys(privKeys);

            var result = menus.ToList();

            var tree = new PrivTreeModel
            {
                Id = "10000"
            };
            BindPrivTree(tree, result);
            return tree.Children;
        }

        private void BindPrivTree(PrivTreeModel treeModel, List<BmsMenus> source)
        {
            var menus = source.Where(p => p.ParentId == treeModel.Id).OrderBy(p => p.OrderId);
            if(menus.Count() == 0)
                return;
            treeModel.Children = new List<PrivTreeModel>();
            foreach(var menu in menus)
            {
                var tree = new PrivTreeModel
                {
                    Id = menu.Id,
                    Title = menu.Name,
                    Type = menu.Type,
                    Icon = menu.Icon,
                    Path = menu.Type == MenuType.Node ? menu.Url : null,
                };
                BindPrivTree(tree, source);
                treeModel.Children.Add(tree);
            }
        }
        #endregion
    }

    public class MenuTreeModel
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public MenuType Type { get; set; }
        public string Icon { get; set; }

        public List<MenuTreeModel> Children { get; set; }
    }

    public class PrivTreeModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string? Path { get; set; }

        public MenuType Type { get; set; }
        public string Icon { get; set; }

        public List<string> Privs { get; set; }

        public List<PrivTreeModel> Children { get; set; }
    }

}
