using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Modules
{
    /// <summary>
    /// RoleIds配置类（用于强类型获取RoleIds）获取RoleId
    /// </summary>
    public class RoleIds
    {
        #region Instance

        private static readonly RoleIds _instance = new RoleIds();

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static RoleIds Instance()
        {
            return _instance;
        }

        private RoleIds()
        { }

        #endregion Instance

        /// <summary>
        /// 超级管理员
        /// </summary>
        public string SuperAdministrator()
        {
            return "101";
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        public string RegisteredUsers()
        {
            return "121";
        }

        /// <summary>
        /// 管制用户
        /// </summary>
        public string ModeratedUser()
        {
            return "123";
        }

        /// <summary>
        /// 匿名用户
        /// </summary>
        public string Anonymous()
        {
            return "122";
        }

        /// <summary>
        /// 受信任用户
        /// </summary>
        public string TrustedUser()
        {
            return "111";
        }
    }
}
