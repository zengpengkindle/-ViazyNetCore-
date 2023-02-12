using ViazyNetCore.Authorization.Models;
using ViazyNetCore.Authorization.Modules.Role.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore;
using ViazyNetCore.Authrozation;
using ViazyNetCore.Caching;
using ViazyNetCore.Dtos;
using ViazyNetCore.Modules;
using ViazyNetCore.Authorization.Modules.Repositories;

namespace ViazyNetCore.Authorization.Modules
{
    public class RoleService
    {
        private readonly ICacheService _cacheService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public RoleService(ICacheService cacheService, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            this._cacheService = cacheService;
            this._roleRepository = roleRepository;
            this._userRoleRepository = userRoleRepository;
        }

        public async Task<IList<string>> GetRoleIdsOfUser(string userId)
        {
            var cacheKeyUserInRole = GetCacheKey_GetRoleIdsOfUser(userId);
            var roleIds = this._cacheService.Get<IList<string>>(cacheKeyUserInRole);
            if (roleIds == null)
            {
                roleIds = await this._roleRepository.GetRoleIdsOfUser(userId);
                this._cacheService.Set(cacheKeyUserInRole, roleIds, CachingExpirationType.ObjectCollection);
            }

            return roleIds;
        }

        public async Task<bool> IsUserInRoles(string userId, params string[] roleIds)
        {
            IEnumerable<string> userRoleIds = await GetRoleIdsOfUser(userId);
            return userRoleIds.Any(r => roleIds.Contains(r));
        }

        public async Task<bool> IsSuperAdministrator(IUser<string> user)
        {
            if (user == null)
                return false;
            if (user.Username == "admin")
            {
                return true;
            }

            return await IsUserInRoles(user.Id, RoleIds.Instance().SuperAdministrator());
        }

        /// <summary>
        /// 把用户加入到一组角色中
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleIds">角色id集合</param>
        public async Task AddUserToRoles(string userId, List<string> roleIds)
        {
            if (roleIds == null)
                return;

            IEnumerable<string> oldRoleNames = await GetRoleIdsOfUser(userId);
            var nameIsChange = roleIds.Except(oldRoleNames).Count() > 0;
            if (nameIsChange)
            {
                await this._userRoleRepository.AddUserToRoles(userId, roleIds);
            }

            var cacheKeyUserInRole = GetCacheKey_GetRoleIdsOfUser(userId);
            this._cacheService.Remove(cacheKeyUserInRole);
        }

        private string GetCacheKey_GetRoleIdsOfUser(string userId)
        {
            return $"RoleIdsOfUser:UserId:{userId}";
        }

        public Task<BmsRole> GetAsync(string roleId)
        {
            return this._roleRepository.GetAsync(roleId);
        }

        public Task<PageData<RoleFindAllModel>> FindRoles(FindRolesParameters args)
        {
            return this._roleRepository.FindAllAsync(args.NameLike, ComStatus.Enabled, new Pagination
            {
                Limit = args.Limit,
                Page = args.Page,
            });
        }

        public async Task<List<RoleSimpleModel>> GetAllAsync()
        {
            return await this._roleRepository.GetAllAsync();
        }

        public Task DeleteRoleAsync(string id)
        {
            return this._roleRepository.UpdateDiy.Set(p => p.Status == ComStatus.Deleted).Where(p => p.Id == id).ExecuteAffrowsAsync();
        }
    }
}
