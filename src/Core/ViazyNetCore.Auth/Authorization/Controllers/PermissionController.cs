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
using ViazyNetCore.Auth.Authorization.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace ViazyNetCore.Authrozation
{
    [Authorize]
    [Permission(PermissionIds.User)]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly IPermissionService _permissionService;

        public PermissionController(RoleService roleService, IPermissionService permissionService)
        {
            this._roleService = roleService;
            this._permissionService = permissionService;
        }

        [HttpGet, Route("getAll")]
        public async Task<List<PermissionKey>> GetAll()
        {
            var result = await this._permissionService.GetAll();

            return result;
        }

        [HttpGet, Route("getRolePermission")]
        public async Task<List<string>> GetRolePermission(string roleId)
        {
            var result = await this._permissionService.GetPermissionsInUserRole(roleId, OwnerType.Role);

            return result.Select(p => p.PermissionItemKey).ToList();
        }

        [HttpPost, Route("updatePermissionsInRole")]
        public async Task<bool> UpdatePermissionsInRole(string roleId, string[] key)
        {
            await _permissionService.UpdatePermissionsInUserRole(key, roleId, OwnerType.Role);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionModel"></param>
        /// <returns></returns>
        [HttpPost, Route("addPermission")]
        public async Task<int> AddPermission(PermissionModel permissionModel)
        {
            await this._permissionService.AddPermission(permissionModel.Name, permissionModel.Key);

            var permission = await this._permissionService.GetPermissionByPermissionKey(permissionModel.Key);
            return permission.Id;
        }

        [HttpPost, Route("getMenuKeysInPermissionKey")]
        public async Task<List<string>> GetMenuKeysInPermissionKey(string permissionKey)
        {
            var result = await this._permissionService.GetMenusInPermissionKey(permissionKey);

            return result.Select(p => p.Id).ToList();
        }

        [HttpPost, Route("updateMenusInPermission")]
        public async Task<bool> UpdateMenusInPermission(string permissionKey, string[] menuIds)
        {
            await this._permissionService.UpdateMenusInPermission(permissionKey, menuIds);
            return true;
        }


        #region 功能菜单管理
        [HttpPost, Route("getMenusTree")]
        public async Task<List<MenuTreeModel>> GetMenusTree()
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
            this.BindTree(tree, result);
            trees.Add(tree);
            return tree.Children;
        }

        [HttpPost, Route("getMenus")]
        public async Task<List<BmsMenus>> GetMenus()
        {
            return await this._permissionService.GetAllMenu();
        }

        [HttpPost, Route("getMenu")]
        public async Task<BmsMenus> GetMenu(string id)
        {
            var result = await this._permissionService.GetMenu(id);
            return result;
        }

        private void BindTree(MenuTreeModel treeModel, List<BmsMenus> source)
        {
            var menus = source.Where(p => p.ParentId == treeModel.Id).OrderBy(p => p.OrderId);
            if (!menus.Any())
                return;
            treeModel.Children = new List<MenuTreeModel>();
            foreach (var menu in menus)
            {
                var tree = new MenuTreeModel
                {
                    Id = menu.Id,
                    Label = menu.Name,
                    Type = menu.Type,
                    Icon = menu.Icon,
                    ParentId = treeModel.Id,
                };
                this.BindTree(tree, source);
                treeModel.Children.Add(tree);
            }
        }

        [HttpPost, Route("updateMenu")]
        public async Task<bool> UpdateMenu(MenusUpdateModel model)
        {
            var item = model.CopyTo<BmsMenus>();
            await this._permissionService.UpdateMenus(item);
            return true;
        }

        [HttpPost, Route("removeMenu")]
        public async Task<bool> RemoveMenu(string menuId)
        {
            await this._permissionService.RemoveMenu(menuId);
            return true;
        }

        #endregion

        #region 菜单

        [Permission(PermissionIds.Anonymity)]
        [HttpPost, Route("getUserMenus")]
        public async Task<List<string>> GetUserMenus()
        {
            var auth = this.HttpContext.GetAuthUser();
            var privs = await this._permissionService.ResolveUserPermission(auth.UserKey);
            var privKeys = privs.Select(p => p.PermissionItemKey).Distinct().ToArray();
            if (auth.UserName == "admin")
            {
                if (!privKeys.Contains(PermissionIds.User))
                    privKeys = privKeys.Concat(new string[] { PermissionIds.User }).ToArray();
            }
            var menus = await this._permissionService.GetMenusInPermissionKeys(privKeys);

            return menus.Select(p => p.Id).ToList();
        }

        [Permission(PermissionIds.Anonymity)]
        [HttpPost, Route("getUserRouters")]
        public async Task<List<PermissionRouterModel>> GetUserRouters()
        {
            var result = await this._permissionService.GetAllMenu();
            var tree = new PermissionRouterModel
            {
                Id = null
            };
            BindPrivTree(tree, result);
            return tree.Children;
        }

        private void BindPrivTree(PermissionRouterModel treeModel, List<BmsMenus> source)
        {
            var menus = source.Where(p => treeModel.Id.IsNullOrEmpty() ? p.ParentId.IsNull() : p.ParentId == treeModel.Id).OrderBy(p => p.OrderId);
            if (menus.Count() == 0)
                return;
            treeModel.Children = new List<PermissionRouterModel>();
            foreach (var menu in menus)
            {
                var tree = new PermissionRouterModel
                {
                    Id = menu.Id,
                    Meta = new PermissionRouteMeta
                    {
                        Icon = menu.Icon,
                        Rank = menu.OrderId,
                        Title = menu.Name,
                        Roles = menu.Type == MenuType.Node ? new List<string> { "admin" } : null,
                    },
                    Name = menu.Type == MenuType.Node ? menu.Description : null,
                    Path = menu.Url,
                };
                BindPrivTree(tree, source);
                treeModel.Children.Add(tree);
            }
        }

        [Permission(PermissionIds.Anonymity)]
        [HttpPost, Route("getUserMenusTree")]
        public async Task<List<PrivTreeModel>> GetUserMenusTree()
        {
            var auth = this.HttpContext.GetAuthUser();
            var privs = await this._permissionService.ResolveUserPermission(auth.UserKey);
            var privKeys = privs.Select(p => p.PermissionItemKey).Distinct().ToArray();
            if (auth.UserName == "admin")
            {
                if (!privKeys.Contains(PermissionIds.User))
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
            if (menus.Count() == 0)
                return;
            treeModel.Children = new List<PrivTreeModel>();
            foreach (var menu in menus)
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
        public string ParentId { get; set; }
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
