using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    [Injection]
    public interface IRoleService
    {
        /// <summary>
        /// 添加或修改模型。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="model">模型。</param>
        /// <returns>模型的编号。</returns>
        Task<long> ManageAsync(RoleModel model);

        /// <summary>
        /// 彻底删除模型。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        Task RemoveAsync(long id);

        /// <summary>
        /// 判定指定的模型编号是否存在。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="id">模型编号。</param>
        /// <returns>存在返回 true 值，否则返回 false 值。</returns>
        Task<bool> ExistsAsync(long id);

        /// <summary>
        /// 查找指定编号的模型。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="id">模型编号。</param>
        /// <returns>模型。</returns>
        Task<RoleModel> FindAsync(long id);
        
        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        Task<PageData<RoleFindAllModel>> FindAllAsync(RoleFindAllArgs args);

        /// <summary>
        /// 查询所有启用的模型。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        Task<PageData<RoleSimpleModel>> FindAllEnabledAsync(RoleFindAllEnabledArgs args);

        /// <summary>
        /// 获取角色的所有页面权限。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="roleId">角色编号。</param>
        /// <returns>页面的编号集合。</returns>
        Task<List<long>> GetRolePagesIdsAsync(long roleId);

        /// <summary>
        /// 设置角色的所有权限页面。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="roleId">角色编号。</param>
        /// <param name="pageIds">页面的编号集合。</param>
        /// <returns>异步操作。</returns>
        Task SetRolePageIdsAsync(long roleId, long[] pageIds);

        /// <summary>
        /// 获取角色的所有接口权限。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="roleId">角色编号。</param>
        /// <returns>接口的编号集合。</returns>
        Task<List<string>> GetRoleApiIdsAsync(long roleId);

        /// <summary>
        /// 设置角色的所有接口权限。
        /// </summary>
        /// <param name="context">数据库上下文。</param>
        /// <param name="roleId">角色编号。</param>
        /// <param name="itemIds">接口的编号集合。</param>
        /// <returns>异步操作。</returns>
        Task SetRoleApiIdsAsync(long roleId, string[] itemIds);

        /// <summary>
        /// 检查角色权限点
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="value">枚举，权限点</param>
        /// <returns></returns>
        Task<bool> CheckPermission(long roleId, object value);
    }
}
