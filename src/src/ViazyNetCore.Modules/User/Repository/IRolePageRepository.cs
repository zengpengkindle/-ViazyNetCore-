using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    [Injection]
    public interface IRolePageRepository : IBaseRepository<BmsRolePage, long>
    {

        /// <summary>
        /// 新增RolePage。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <param name="pageId">页面编号。</param>
        /// <param name="createTime">创建时间。</param>
        /// <returns></returns>
        Task AddRolePageAsync(long roleId, long pageId, DateTime createTime);

        /// <summary>
        /// 根据roleId移除RolePage。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <returns></returns>
        Task RemoveRolePageByIdAsync(long roleId);

        /// <summary>
        /// 获取角色的所有页面权限。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <returns>页面的编号集合。</returns>
        public Task<List<long>> GetRolePagesIdsAsync(long roleId);
    }
}