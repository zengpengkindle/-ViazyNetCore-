using ViazyNetCore.Authorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore;
using ViazyNetCore.Caching;
using ViazyNetCore.Modules;
using ViazyNetCore.Authorization.Modules.Repositories;

namespace ViazyNetCore.Authorization.Modules
{
    [Injection]
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

        public async Task<List<long>> GetRoleIdsOfUserByCache(long userId)
        {
            var cacheKeyUserInRole = GetCacheKey_GetRoleIdsOfUser(userId);
            var roleIds = await this._cacheService.LockGetAsync(cacheKeyUserInRole
                , () => this._userRoleRepository.GetRoleIdsOfUser(userId)
                , CachingExpirationType.ObjectCollection);

            return roleIds;
        }

        public async Task<bool> IsUserInRoles(long userId, params long[] roleIds)
        {
            IEnumerable<long> userRoleIds = await GetRoleIdsOfUserByCache(userId);
            return userRoleIds.Any(r => roleIds.Contains(r));
        }

        public async Task<bool> IsSuperAdministrator(IUser<long> user)
        {
            if (user == null)
                return false;
            if (user.Username == "Administrator")
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
        public async Task UpdateUserToRoles(long userId, List<long> roleIds)
        {
            if (roleIds == null)
                return;

            IEnumerable<long> oldRoleNames = await GetRoleIdsOfUserByCache(userId);
            var nameIsChange = roleIds.Except(oldRoleNames).Count() > 0;
            if (nameIsChange)
            {
                await this._userRoleRepository.UpdateUserToRoles(userId, roleIds);
            }

            var cacheKeyUserInRole = GetCacheKey_GetRoleIdsOfUser(userId);
            this._cacheService.Remove(cacheKeyUserInRole);
        }

        private string GetCacheKey_GetRoleIdsOfUser(long userId)
        {
            return $"RoleIdsOfUser:UserId:{userId}";
        }

        public Task<BmsRole> GetAsync(long roleId)
        {
            return this._roleRepository.GetAsync(roleId);
        }

        public Task<PageData<RoleQueryDto>> FindRoles(RolePageQueryDto args, Pagination pagination)
        {
            return this._roleRepository.FindAllAsync(args.NameLike, ComStatus.Enabled, pagination);
        }

        public async Task<List<RoleNameDto>> GetAllAsync()
        {
            return await this._roleRepository.GetAllAsync();
        }

        public Task DeleteRoleAsync(long id)
        {
            return this._roleRepository.UpdateDiy.Set(p => p.Status == ComStatus.Deleted).Where(p => p.Id == id).ExecuteAffrowsAsync();
        }

        public Task UpdateAsync(BmsRole item)
        {
            return this._roleRepository.Orm.InsertOrUpdate<BmsRole>()
                .SetSource(item)
                .UpdateColumns(p => new { p.Name, p.UpdateUserId, p.UpdateUserName, p.UpdateTime, p.Status, p.ExtraData })
                .ExecuteAffrowsAsync();
        }

        public Task<BmsRole> GetByNameAsync(string roleName)
        {
            return this._roleRepository.Where(p => p.Name == roleName).FirstAsync();
        }
    }
}
