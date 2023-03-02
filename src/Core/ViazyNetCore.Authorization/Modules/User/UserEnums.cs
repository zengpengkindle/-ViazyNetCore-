using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Modules
{

    /// <summary>
    /// 用户登录状态
    /// </summary>
    public enum UserLoginStatus
    {
        /// <summary>
        /// 通过身份验证，登录成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 用户名、密码不匹配
        /// </summary>
        InvalidCredentials = 1,
        /// <summary>
        /// 帐户未激活
        /// </summary>
        NotActivated = 2,

        /// <summary>
        /// 帐户被封禁
        /// </summary>
        Banned = 3,

        /// <summary>
        /// 不允许手机登录
        /// </summary>
        InvalidMobile = 4,

        /// <summary>
        /// 不允许邮箱登录
        /// </summary>
        InvalidEmail = 5,
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError = 100
    }
}
