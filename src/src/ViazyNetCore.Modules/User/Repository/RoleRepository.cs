using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个角色的服务仓储。
    /// </summary>
    [Injection]
    public class RoleRepository : DefaultRepository<BmsRole, long>, IRoleRepository
    {
        private const string ROLE_ROLEMODEL_ID = "ROLE_ROLEMODEL_ID_{0}";
        private readonly IRolePageRepository _rolePageRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;

        public RoleRepository(IFreeSql freeSql, IUserRepository userRepository, IRolePageRepository rolePageRepository, IRolePermissionRepository rolePermissionRepository, ICacheService cacheService) : base(freeSql)
        {
            this._rolePageRepository = rolePageRepository;
            this._rolePermissionRepository = rolePermissionRepository;
            this._userRepository = userRepository;
            this._cacheService = cacheService;
        }

        #region 新增
        /// <summary>
        /// 根据roleId新增用户权限。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <param name="itemId">项目编号。</param>
        /// <param name="createTime">创建时间。</param>
        /// <returns></returns>
        public async Task AddRolePermissionByIdAsync(long roleId, string itemId, DateTime createTime)
        {
            //删除缓存，及时生效
            this.ClearPermissionIdsByRoleIdCache(roleId);
            await this._rolePermissionRepository.InsertAsync(new BmsRolePermission
            {
                RoleId = roleId,
                ItemId = itemId,
                CreateTime = createTime,
            });
        }

        /// <summary>
        /// 根据BmsRole新增
        /// </summary>
        /// <param name="role">BmsRole模型</param>
        /// <returns></returns>
        public async Task AddByBmsRoleAsync(BmsRole role)
        {
            await this.InsertAsync(role);
        }

        #endregion

        #region 更新
        /// <summary>
        /// 根据RoleModel修改
        /// </summary>
        /// <param name="model">RoleModel模型</param>
        /// <returns></returns>
        public async Task ModifyByRoleModelAsync(RoleModel model)
        {
            //重置缓存
            this.ClearPermissionIdsByRoleIdCache(model.Id);
            await this.UpdateDiy
                    .Where(pg => pg.Id == model.Id)
                    .SetDto(new
                    {
                        model.Name,
                        model.Status,
                        ModifyTime = DateTime.Now,
                        model.ExtraData,
                    }).ExecuteAffrowsAsync();
        }

        #endregion

        #region 删除
        /// <summary>
        /// 根据角色编号移除角色。
        /// </summary>
        /// <param name="id">角色编号。</param>
        /// <returns>异步操作。</returns>
        public Task RemoveByIdAsync(long id)
        {
            //重置缓存
            this.ClearPermissionIdsByRoleIdCache(id);
            return this.DeleteAsync(r => r.Id == id);
        }

        #endregion

        #region 查询

        /// <summary>
        /// 判定指定的模型编号是否存在。
        /// </summary>
        /// <param name="id">模型编号。</param>
        /// <returns>存在返回 true 值，否则返回 false 值。</returns>
        public Task<bool> ExistsAsync(long id)
        {
            return this.Select.AnyAsync(r => r.Id == id);
        }

        /// <summary>
        /// 查找指定编号的模型。
        /// </summary>
        /// <param name="id">模型编号。</param>
        /// <returns>模型。</returns>
        public Task<RoleModel> FindByIdAsync(long id)
        {
            return this._cacheService.GetAsync(string.Format(ROLE_ROLEMODEL_ID, id), () =>
            {
                return this.Select.Where(r => r.Id == id).WithTempQuery(r => new RoleModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Status = r.Status,
                    ExtraData = r.ExtraData,
                }).FirstAsync();
            });
        }

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="nameLike">名称模糊搜索</param>
        /// <param name="status">状态</param>
        /// <param name="args">分页参数</param>
        /// <returns></returns>
        public Task<PageData<RoleFindAllModel>> FindAllAsync(string nameLike, ComStatus? status, Pagination args)
        {
            var query = this.Select;
            if(nameLike.IsNotNull()) query = query.Where(r => r.Name.Contains(nameLike));
            if(status.HasValue) query = query.Where(r => r.Status == status.Value);
            else query = query.Where(a => a.Status != ComStatus.Deleted);

            return query
                .OrderByDescending(r => r.ModifyTime)
                .WithTempQuery(r => new RoleFindAllModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Status = r.Status,
                    CreateTime = r.CreateTime,
                    ModifyTime = r.ModifyTime,
                })
                .ToPageAsync(args);
        }

        ///// <summary>
        ///// 查询所有启用的模型。
        ///// </summary>
        ///// <param name="nameLike">名称模糊搜索</param>
        ///// <param name="args">分页参数</param>
        ///// <returns></returns>
        //public Task<PageData<RoleSimpleModel>> FindAllEnabledAsync(string nameLike, Pagination args)
        //{
        //    var query = this.Table.Where(r => r.Status == ComStatus.Enabled);
        //    if(nameLike.IsNotNull()) query = query.Where(r => r.Name.Contains(nameLike));
        //    return query
        //        .OrderBy(r => r.Name)
        //        .Select(r => new RoleSimpleModel
        //        {
        //            Id = r.Id,
        //            Name = r.Name,
        //        })
        //        .ToPageAsync(args);
        //}

        #endregion

        #region 缓存

        /// <summary>
        /// 获取角色的所有页面权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetPermissionIdsByRoleIdCacheAsync(long roleId)
        {
            var cacheKey = this.GetPermissionIdsByRoleIdCacheKey(roleId);
            var result = this._cacheService.Get<List<string>>(cacheKey);
            if(result == null)
            {
                result = await this.Orm.Select<BmsRolePermission>().Where(rp => rp.RoleId == roleId)
                .WithTempQuery(rp => rp.ItemId)
                .ToListAsync(); ;
                if(result != null)
                    this._cacheService.Set(cacheKey, result, CachingExpirationType.ObjectCollection);
            }
            return result;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <returns></returns>
        public void ClearPermissionIdsByRoleIdCache(long roleId)
        {
            this._cacheService.Remove(this.GetPermissionIdsByRoleIdCacheKey(roleId));
            this._cacheService.Remove(string.Format(ROLE_ROLEMODEL_ID, roleId));
        }

        private string GetPermissionIdsByRoleIdCacheKey(long roleId)
        {
            return $"ROLEAPIS_CACHE_KEY_{roleId}";
        }

        #endregion
    }
}
