using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth
{
    /// <summary>
    /// 用户密码存储格式
    /// </summary>
    public enum UserPasswordFormat
    {
        /// <summary>
        /// 密码未加密
        /// </summary>
        Clear = 0,

        /// <summary>
        /// 标准MD5加密
        /// </summary>
        MD5 = 1,
    }

    /// <summary>
    /// 用户密码辅助工具类
    /// </summary>
    public class UserPasswordHelper
    {
        /// <summary>
        /// 检查用户密码是否正确
        /// </summary>
        /// <param name="password">用户录入的用户密码（尚未加密的密码）</param>
        /// <param name="storedPassword">数据库存储的密码（即加密过的密码）</param>
        /// <param name="passwordFormat">用户密码存储格式</param>
        public static bool CheckPassword(string password, string storedPassword, Guid slat, UserPasswordFormat passwordFormat)
        {
            if(password.IsNull()) return false;
            return EncodePassword(password, slat, passwordFormat) == storedPassword;
        }

        /// <summary>
        /// 对用户密码进行编码
        /// </summary>
        /// <param name="password">需要加密的用户密码</param>
        /// <param name="passwordFormat">用户密码存储格式</param>
        public static string EncodePassword(string password, Guid slat, UserPasswordFormat passwordFormat)
        {
            switch(passwordFormat)
            {
                case UserPasswordFormat.Clear:
                    return password;
                case UserPasswordFormat.MD5:
                    return DataSecurity.GenerateSaltedHash(password, slat);
                default:
                    break;
            }
            throw new NotSupportedException($"不支持 {passwordFormat} 的密码加密方式。");
        }
    }
}
