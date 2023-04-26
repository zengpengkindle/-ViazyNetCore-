using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ViazyNetCore.Auth.Authorization.ViewModels;
using ViazyNetCore.Authorization.Modules;

namespace ViazyNetCore.Authrozation
{
    [Permission(PermissionIds.User)]
    public class PermissionController : DynamicControllerBase
    {
        private readonly RoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionController(RoleService roleService, IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
        {
            this._roleService = roleService;
            this._permissionService = permissionService;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<List<PermissionKey>> GetAll()
        {
            var result = await this._permissionService.GetAll();

            return result;
        }

        [HttpGet]
        public async Task<List<string>> GetRolePermission(string roleId)
        {
            var result = await this._permissionService.GetPermissionsInUserRole(roleId, OwnerType.Role);

            return result.Select(p => p.PermissionItemKey).ToList();
        }

        [HttpPost]
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
        [HttpPost]
        public async Task<int> AddPermission(PermissionModel permissionModel)
        {
            await this._permissionService.AddPermission(permissionModel.Name, permissionModel.Key);

            var permission = await this._permissionService.GetPermissionByPermissionKey(permissionModel.Key);
            return permission.Id;
        }

        [HttpPost]
        public async Task<List<string>> GetMenuKeysInPermissionKey(string permissionKey)
        {
            var result = await this._permissionService.GetMenusInPermissionKey(permissionKey);

            return result.Select(p => p.Id).ToList();
        }

        [HttpPost]
        public async Task<bool> UpdateMenusInPermission(string permissionKey, string[] menuIds)
        {
            await this._permissionService.UpdateMenusInPermission(permissionKey, menuIds);
            return true;
        }


        #region 功能菜单管理
        [HttpPost]
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

        [HttpPost]
        public async Task<List<BmsMenus>> GetMenus()
        {
            return await this._permissionService.GetAllMenu();
        }

        /// <summary>
        /// 获取菜单明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
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

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UpdateMenu(MenusUpdateModel model)
        {
            var item = model.CopyTo<BmsMenus>();
            await this._permissionService.UpdateMenus(item);
            return true;
        }

        /// <summary>
        /// 移除菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> RemoveMenu(string menuId)
        {
            await this._permissionService.RemoveMenu(menuId);
            return true;
        }

        #endregion

        #region 菜单

        [Permission(PermissionIds.Anonymity)]
        [HttpPost]
        public async Task<List<string>> GetUserMenus()
        {
            var auth = this._httpContextAccessor.HttpContext!.GetAuthUser();
            var privs = await this._permissionService.ResolveUserPermission(auth.Id);
            var privKeys = privs.Select(p => p.PermissionItemKey).Distinct().ToArray();
            if (auth.Username == "admin")
            {
                if (!privKeys.Contains(PermissionIds.User))
                    privKeys = privKeys.Concat(new string[] { PermissionIds.User }).ToArray();
            }
            var menus = await this._permissionService.GetMenusInPermissionKeys(privKeys);

            return menus.Select(p => p.Id).ToList();
        }


        /// <summary>
        /// 获取当前用户菜单数组
        /// </summary>
        /// <returns></returns>
        [Permission(PermissionIds.Anonymity)]
        [HttpPost]
        public async Task<List<PermissionRouterModel>?> GetUserRouters()
        {
            var result = await this._permissionService.GetAllEnableMenu();
            var tree = new PermissionRouterModel();
            BindPrivTree(tree, result);
            return tree.Children;
        }

        private void BindPrivTree(PermissionRouterModel treeModel, List<BmsMenus> source)
        {
            var menus = source.Where(p => treeModel.Id.IsNullOrEmpty() ? p.ParentId.IsNull() : p.ParentId == treeModel.Id).OrderBy(p => p.OrderId);
            if (menus.Count() == 0)
                return;
            if (menus.Any(P => P.Type == MenuType.Button))
            {
                treeModel.Meta.Auths = menus.Where(p => p.Type == MenuType.Button).Select(p => p.Description).ToList();
            }

            if (!menus.Where(P => P.Type != MenuType.Button).Any())
                return;
            treeModel.Children = new List<PermissionRouterModel>();
            foreach (var menu in menus.Where(P => P.Type != MenuType.Button))
            {
                var tree = new PermissionRouterModel
                {
                    Id = menu.Id,
                    Meta = new PermissionRouteMeta
                    {
                        Icon = menu.Icon,
                        Rank = menu.OrderId,
                        Title = menu.Name,
                        //Roles = menu.Type == MenuType.Node ? new List<string> { "admin" } : null,
                    },
                    Name = menu.Type == MenuType.Node ? menu.Description : null,
                    Path = menu.Url,
                };
                BindPrivTree(tree, source);
                treeModel.Children.Add(tree);
            }
        }

        /// <summary>
        /// 获取当前用户菜单树
        /// </summary>
        /// <returns></returns>
        [Permission(PermissionIds.Anonymity)]
        [HttpPost]
        public async Task<List<PrivTreeModel>> GetUserMenusTree()
        {
            var auth = this._httpContextAccessor.HttpContext!.GetAuthUser();
            var privs = await this._permissionService.ResolveUserPermission(auth.Id);
            var privKeys = privs.Select(p => p.PermissionItemKey).Distinct().ToArray();
            if (auth.Username == "admin")
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
}
