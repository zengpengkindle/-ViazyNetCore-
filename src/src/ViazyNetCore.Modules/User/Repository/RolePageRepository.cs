using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个角色的服务仓储。
    /// </summary>
    [Injection]
    public class RolePageRepository : DefaultRepository<BmsRolePage,long>, IRolePageRepository
    {
        public RolePageRepository(IFreeSql fsql) : base(fsql)
        {
        }

        #region 新增

        /// <summary>
        /// 新增RolePage。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <param name="pageId">页面编号。</param>
        /// <param name="createTime">创建时间。</param>
        /// <returns></returns>
        public async Task AddRolePageAsync(long roleId, long pageId, DateTime createTime)
        {
            await this.InsertAsync(new BmsRolePage
            {
                RoleId = roleId,
                PageId = pageId,
                CreateTime = createTime,
            });
        }

        #endregion

        #region 删除

        /// <summary>
        /// 根据roleId移除RolePage。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <returns></returns>
        public async Task RemoveRolePageByIdAsync(long roleId)
        {
            await this.DeleteAsync(rp => rp.RoleId == roleId);
        }

        #endregion

        #region 查询


        /// <summary>
        /// 获取角色的所有页面权限。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <returns>页面的编号集合。</returns>
        public Task<List<long>> GetRolePagesIdsAsync(long roleId)
        {
            return this.Select.Where(rp => rp.RoleId == roleId)
                .WithTempQuery(rp => rp.PageId)
                .ToListAsync();
        }

        #endregion

    }
}
