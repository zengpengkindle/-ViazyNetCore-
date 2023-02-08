using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个角色的服务仓储接口。
    /// </summary>
    [Injection]
    public interface IRoleRepository : IBaseRepository<BmsRole, long>
    {
        /// <summary>
        /// 根据BmsRole新增
        /// </summary>
        /// <param name="role">BmsRole模型</param>
        /// <returns></returns>
        Task AddByBmsRoleAsync(BmsRole role);

        /// <summary>
        /// 根据RoleModel修改
        /// </summary>
        /// <param name="model">RoleModel模型</param>
        /// <returns></returns>
        Task ModifyByRoleModelAsync(RoleModel model);

        /// <summary>
        /// 根据角色编号移除角色。
        /// </summary>
        /// <param name="id">角色编号。</param>
        /// <returns>异步操作。</returns>
        Task RemoveByIdAsync(long id);

        /// <summary>
        /// 判定指定的模型编号是否存在。
        /// </summary>
        /// <param name="id">模型编号。</param>
        /// <returns>存在返回 true 值，否则返回 false 值。</returns>
        Task<bool> ExistsAsync(long id);

        /// <summary>
        /// 查找指定编号的模型。
        /// </summary>
        /// <param name="id">模型编号。</param>
        /// <returns>模型。</returns>
        Task<RoleModel> FindByIdAsync(long id);

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="nameLike">名称模糊搜索</param>
        /// <param name="status">状态</param>
        /// <param name="args">分页参数</param>
        /// <returns></returns>
        Task<PageData<RoleFindAllModel>> FindAllAsync(string nameLike, ComStatus? status, Pagination args);

        /// <summary>
        /// 根据roleId新增用户权限。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <param name="itemId">项目编号。</param>
        /// <param name="createTime">创建时间。</param>
        /// <returns></returns>
        Task AddRolePermissionByIdAsync(long roleId, string itemId, DateTime createTime);

        /// <summary>
        /// 获取角色的所有页面权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<string>> GetPermissionIdsByRoleIdCacheAsync(long roleId);

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="roleId"></param>
        void ClearPermissionIdsByRoleIdCache(long roleId);

    }
}
