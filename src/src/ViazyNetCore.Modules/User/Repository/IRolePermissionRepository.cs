using System;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个角色权限的仓储接口。
    /// </summary>
    [Injection]
    public interface IRolePermissionRepository : IBaseRepository<BmsRolePermission, long>
    {
        /// <summary>
        /// 根据roleId移除用户权限。
        /// </summary>
        /// <param name="roleId">角色编号。</param>
        /// <returns></returns>
        Task RemoveRolePermissionByIdAsync(long roleId);
    }
}