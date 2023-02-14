﻿using Microsoft.Extensions.Caching.Distributed;
using ViazyNetCore.Authorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules;
using ViazyNetCore.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Authorization.Modules
{
    [Injection]
    public class PermissionService: IPermissionService
    {
        private readonly IFreeSql _freeSql;
        private readonly IUserService _userService;
        private readonly RoleService _roleService;
        private readonly ICacheService _cacheService;
        private readonly UnitOfWorkManager _uowm;

        public PermissionService(IFreeSql freeSql, IUserService userService, RoleService roleService, ICacheService cacheService, UnitOfWorkManager uowm)
        {
            this._freeSql = freeSql;
            this._userService = userService;
            this._roleService = roleService;
            this._cacheService = cacheService;
            this._uowm = uowm;
        }

        public Task<List<PermissionKey>> GetAll()
        {
            return this._freeSql.Select<BmsPermission>().WithTempQuery(p => new PermissionKey { Key = p.PermissionId, Name = p.Name, Id = p.Id }).ToListAsync();
        }

        /// <summary>
        /// 获取用户角色对应的权限设置
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="ownerType">拥有者所属类别</param>
        /// <returns>返回roleName对应的权限设置</returns>
        public async Task<IEnumerable<BmsOwnerPermission>> GetPermissionsInUserRole(string ownerId, OwnerType ownerType)
        {
            string cacheKey = GetCacheKey_GetPermissionsInUserRole(ownerId, ownerType);
            var permissionItemInUserRoles = this._cacheService.Get<List<BmsOwnerPermission>>(cacheKey);

            if (permissionItemInUserRoles == null)
            {
                permissionItemInUserRoles = await this._freeSql.Select<BmsOwnerPermission>()
                    .Where(p => p.OwnerId == ownerId && p.OwnerType == ownerType).ToListAsync();
                this._cacheService.Set(cacheKey, permissionItemInUserRoles, CachingExpirationType.ObjectCollection);
            }
            return permissionItemInUserRoles;
        }

        /// <summary>
        /// 更新权限规则
        /// </summary>
        /// <param name="permissionItemKeys">待更新的权限项目规则集合</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="ownerType">拥有者所属类别</param>
        public async Task UpdatePermissionsInUserRole(IEnumerable<string> permissionItemKeys, string ownerId, OwnerType ownerType)
        {
            using (var context = _uowm.Begin())
            {
                await this._freeSql.Select<BmsOwnerPermission>()
                    .Where(p => p.OwnerId == ownerId && p.OwnerType == ownerType && !p.IsLock).ToDelete().ExecuteAffrowsAsync();
                var permissions = await this._freeSql.Select<BmsOwnerPermission>().Where(p => p.OwnerId == ownerId && p.OwnerType == ownerType).ToListAsync();
                if (permissionItemKeys != null)
                {
                    foreach (var permissionItemKey in permissionItemKeys)
                    {
                        //获取的权限等于空或者不包含当前要更改的权限项
                        if (permissions == null || (permissions != null && !permissions.Select(n => n.PermissionItemKey).Contains(permissionItemKey)))
                        {
                            await this._freeSql.Insert<BmsOwnerPermission>().MySqlIgnoreInto().AppendData(new BmsOwnerPermission
                            {
                                Id = Snowflake.NextIdString(),
                                IsLock = false,
                                Status = ComStatus.Enabled,
                                OwnerId = ownerId,
                                OwnerType = ownerType,
                                PermissionItemKey = permissionItemKey
                            }).ExecuteAffrowsAsync();
                        }
                    }
                }
                context.Commit();
            }
            string cacheKey = GetCacheKey_GetPermissionsInUserRole(ownerId, ownerType);
            this._cacheService.Remove(cacheKey);
            //更新缓存
            await GetPermissionsInUserRole(ownerId, ownerType);
        }

        public async Task RemoveMenu(string menuId)
        {
            await this._freeSql.Select<BmsMenus>().Where(p => p.Id == menuId).ToDelete().ExecuteAffrowsAsync();

            var permissionItemKeys = await this._freeSql.Select<BmsPermissionMenu>().Where(p => p.MenuId == menuId).WithTempQuery(p => p.PermissionItemKey).ToListAsync();
            foreach (var permissionItemKey in permissionItemKeys)
            {
                //更新缓存
                string cacheKey = GetCacheKey_GetMenusInPermissionKey(permissionItemKey);
                this._cacheService.Remove(cacheKey);
            }
        }

        public Task<List<BmsMenus>> GetAllMenu()
        {
            return this._freeSql.Select<BmsMenus>().ToListAsync();
        }

        public Task<BmsMenus> GetMenu(string id)
        {
            return this._freeSql.Select<BmsMenus>().Where(p => p.Id == id).FirstAsync();
        }

        public async Task<string> UpdateMenus(BmsMenus menu)
        {
            if (menu.Id.IsNull())
            {
                menu.Id = Snowflake.NextIdString();
                menu.Status = ComStatus.Enabled;
                menu.CreateTime = DateTime.Now;
                await this._freeSql.Insert(menu).ExecuteAffrowsAsync();
                return menu.Id;
            }
            else
            {
                menu.Status = ComStatus.Enabled;
                await this._freeSql.Update<BmsMenus>(new
                {
                    menu.Id,
                    menu.Name,
                    menu.Type,
                    menu.Icon,
                    menu.Description,
                    OpenType = 0,
                    menu.IsHomeShow,
                    Url = menu.Type == MenuType.Node ? menu.Url : null,
                    menu.OrderId,
                    Status = ComStatus.Enabled
                }).ExecuteAffrowsAsync();
                return menu.Id;
            }
        }

        public async Task AddPermission(string name, string key)
        {
            var exists = await ExistsPermissionByPermissionKey(key);
            if (exists)
            {
                throw new ApiException("已存在键名");
            }
            await this._freeSql.Insert(new BmsPermission
            {
                PermissionId = key,
                Name = name
            }).ExecuteAffrowsAsync();
        }

        public Task<BmsPermission> GetPermissionByPermissionKey(string key)
        {
            return this._freeSql.Select<BmsPermission>().Where(p => p.PermissionId == key)
                .FirstAsync();
        }

        public Task<bool> ExistsPermissionByPermissionKey(string key)
        {
            return this._freeSql.Select<BmsPermission>().Where(p => p.PermissionId == key)
                .AnyAsync();
        }

        #region Menus
        public async Task UpdateMenusInPermission(string permissionItemKey, string[] menuIds)
        {

            await this._freeSql.Select<BmsPermissionMenu>()
                .Where(p => p.PermissionItemKey == permissionItemKey).ToDelete().ExecuteAffrowsAsync();

            if (menuIds != null)
            {
                foreach (var menuId in menuIds)
                {
                    //获取的权限等于空或者不包含当前要更改的权限项

                    await this._freeSql.Insert(new BmsPermissionMenu
                    {
                        Id = Snowflake.NextIdString(),
                        MenuId = menuId,
                        PermissionItemKey = permissionItemKey
                    }).ExecuteAffrowsAsync();
                }
            }
            //更新缓存
            string cacheKey = GetCacheKey_GetMenusInPermissionKey(permissionItemKey);
            this._cacheService.Remove(cacheKey);
            await GetMenusInPermissionKey(permissionItemKey);
        }

        public async Task<IEnumerable<BmsMenus>> GetMenusInPermissionKey(string permissionItemKey)
        {
            string cacheKey = GetCacheKey_GetMenusInPermissionKey(permissionItemKey);
            var bmsMenus = this._cacheService.Get<List<BmsMenus>>(cacheKey);

            if (bmsMenus == null)
            {
                bmsMenus =await this._freeSql.Select<BmsMenus>().From<BmsPermissionMenu>().InnerJoin((menu, p) => p.MenuId == menu.Id)
                   .Where((menu, pm) => pm.PermissionItemKey == permissionItemKey)
                   .WithTempQuery((menu, pm) => menu)
                   .ToListAsync();
                this._cacheService.Set(cacheKey, bmsMenus, CachingExpirationType.ObjectCollection);
            }
            return bmsMenus;
        }

        public async Task<IEnumerable<BmsMenus>> GetMenusInPermissionKeys(string[] permissionItemKeys)
        {
            //var query = from menu in this._engine.Table<BmsMenus>()
            //            from pm in this._engine.Table<BmsPermissionMenu>().InnerJoin(p => p.MenuId == menu.Id)
            //            where permissionItemKeys.Contains(pm.PermissionItemKey)
            //            select menu;
            //return query.Distinct().AsEnumerableAsync();

            //修改为根据单个查询，记录缓存。
            IEnumerable<BmsMenus> bmsMenus = new List<BmsMenus>();
            foreach (var key in permissionItemKeys)
            {
                var bmsMenusByPermissionKey = await GetMenusInPermissionKey(key);
                bmsMenus = bmsMenus.Union(bmsMenusByPermissionKey);
            }
            return bmsMenus;
        }

        #endregion

        /// <summary>
        /// 解析用户的所有权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<BmsOwnerPermission>> ResolveUserPermission(string userId)
        {
            IEnumerable<BmsOwnerPermission> permissions = new List<BmsOwnerPermission>();
            var user =await _userService.GetUser(userId);
            //匿名用户
            if (user == null)
                return permissions;

            IList<string> roleIdsOfUser = await _roleService.GetRoleIdsOfUser(userId);

            //if(user.IsModerated)
            //    roleIdsOfUser.Add(RoleIds.Instance().ModeratedUser());

            foreach (var roleId in roleIdsOfUser)
            {
                var rolePermissions = await GetPermissionsInUserRole(roleId, OwnerType.Role);
                permissions = permissions.Union(rolePermissions);
            }
            var userPermissions = await GetPermissionsInUserRole(userId, OwnerType.User);
            permissions = permissions.Union(userPermissions);
            return permissions;
        }

        /// <summary>
        /// 检查用户是否有权限进行某项操作
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="permissionItemKey">权限项目标识</param>
        /// <returns>有权限操作返回true，否则返回false</returns>
        public async Task<bool> Check(IUser<string> currentUser, string permissionItemKey)
        {
            if (currentUser == null)
                return false;

            if (await this._roleService.IsSuperAdministrator(currentUser))
                return true;
            //获取用户的所有权限
            var resolvedUserPermission = await ResolveUserPermission(currentUser.Id);
            if (resolvedUserPermission == null)
                return false;
            //判断用户的所有权限里有没有当前权限
            return resolvedUserPermission.Select(n => n.PermissionItemKey).Contains(permissionItemKey);
        }

        /// <summary>
        /// 检查用户是否有权限进行某项操作
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="permissionItemKey">权限项目标识</param>
        /// <returns>有权限操作返回true，否则返回false</returns>
        public async Task<bool> Check(IUser currentUser, string[] permissionItemKeys)
        {
            if (currentUser == null)
                return false;

            if (await this._roleService.IsSuperAdministrator(currentUser))
                return true;
            //获取用户的所有权限
            var resolvedUserPermission = await ResolveUserPermission(currentUser.Id);
            if (resolvedUserPermission == null)
                return false;
            //判断用户的所有权限里有没有当前权限
            return resolvedUserPermission.Select(n => n.PermissionItemKey).Intersect(permissionItemKeys).Any();
        }

        private string GetCacheKey_GetPermissionsInUserRole(string ownerId, OwnerType ownerType)
        {
            return string.Format("PermissionItemsInUserRole:RoleId:{0}:OwnerType:{1}", ownerId, ownerType);
        }

        private string GetCacheKey_GetMenusInPermissionKey(string permissionItemKey)
        {
            return string.Format("MenusInPermissionKey:PermissionItemKey:{0}", permissionItemKey);
        }
    }
}
