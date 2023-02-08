using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个角色权限的仓储。
    /// </summary>
    [Injection]
    public class RolePermissionRepository : DefaultRepository<BmsRolePermission,long>, IRolePermissionRepository
    {
        public RolePermissionRepository(IFreeSql fsql) : base(fsql)
        {
        }
        #region 删除

        /// <summary>
        /// 根据roleId移除用户权限。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <returns></returns>
        public async Task RemoveRolePermissionByIdAsync(long roleId)
        {
            await this.DeleteAsync(rp => rp.RoleId == roleId);
        }

        #endregion


    }
}
