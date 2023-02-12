using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Modules
{
    [Injection]
    public interface IAuthorizationService
    {
        /// <summary>
        /// 当前用户是不是超级管理员
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <returns>是超级管理员返回true，否则返回false</returns>
        Task<bool> IsSuperAdministrator(IUser currentUser);

        /// <summary>
        /// 检查用户是否有权限进行某项操作
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="permissionItemKey">权限项目标识</param>
        /// <returns>有权限操作返回true，否则返回false</returns>
        Task<bool> Check(IUser currentUser, string permissionItemKey);
    }
}
