using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Modules
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录的用户</param>
        /// <param name="rememberPassword">是否记住密码(是否长时登录)</param>
        Task<string> SignIn(string username, string password, Guid securityCode);

        /// <summary>
        /// 注销
        /// </summary>
        void SignOut();

        /// <summary>
        /// 获取当前登录的用户
        /// </summary>
        /// <returns>
        /// 当前用户未通过认证则返回null
        /// </returns>
        IUser<string> GetAuthenticatedUser();
    }
}
