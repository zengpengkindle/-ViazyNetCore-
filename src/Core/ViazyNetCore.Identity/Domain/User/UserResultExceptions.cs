using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore;

namespace ViazyNetCore.Authorization.Modules
{
    public static class UserCreateExceptions
    {
        private const int UserExceptionStartCode = 10000;
        /// <summary>
        /// 未知错误
        /// </summary>
        public static void Unknown()
        {
            throw new ApiException("未知错误", UserExceptionStartCode + 1);
        }

        /// <summary>
        /// 用户名重复
        /// </summary>
        public static void DuplicateUsername()
        {
            throw new ApiException("用户名重复", UserExceptionStartCode + 2);
        }

        /// <summary>
        /// Email重复
        /// </summary>
        public static void DuplicateEmailAddress()
        {
            throw new ApiException("Email重复", UserExceptionStartCode + 3);
        }

        /// <summary>
        /// 手机号重复
        /// </summary>
        public static void DuplicateMobile()
        {
            throw new ApiException("手机号重复", UserExceptionStartCode + 4);
        }

        /// <summary>
        /// 不允许的用户名
        /// </summary>
        public static void DisallowedUsername()
        {
            throw new ApiException("不允许的用户名", UserExceptionStartCode + 5);
        }

        /// <summary>
        /// 无效邀请码
        /// </summary>
        public static void InviteInvalid()
        {
            throw new ApiException("无效邀请码", UserExceptionStartCode + 6);
        }
    }

    public static class UserDeleteExceptions
    {
        private const int UserExceptionStartCode = 10100;
        /// <summary>
        /// 未知错误
        /// </summary>
        public static void Unknown()
        {
            throw new ApiException("未知错误", UserExceptionStartCode + 1);
        }

        /// <summary>
        /// 接管被删除用户内容的用户名不存在
        /// </summary>
        public static void InvalidTakeOverUsername()
        {
            throw new ApiException("接管被删除用户内容的用户名不存在", UserExceptionStartCode + 2);
        }

        /// <summary>
        /// 待删除的用户不存在
        /// </summary>
        public static void DeletingUserNotFound()
        {
            throw new ApiException("待删除的用户不存在", UserExceptionStartCode + 3);
        }

    }
}
