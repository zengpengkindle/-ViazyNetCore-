using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit.Modules
{
    public class PermissionItemKeys
    {
        private static readonly PermissionItemKeys _instance = new PermissionItemKeys();

        /// <summary>
        /// 获取该类的单例
        /// </summary>
        /// <returns></returns>
        public static PermissionItemKeys Instance()
        {
            return _instance;
        }

        private PermissionItemKeys()
        { }
    }

    public static class PermissionItemKeysExtension
    {

        /// <summary>
        /// 用户、角色、等级管理，权限管理、第三方登录设置、操作日志浏览及清除、积分记录浏览
        /// </summary>
        /// <param name="pik"><see cref="PermissionItemKeys"/></param>
        /// <returns></returns>
        public static string User(this PermissionItemKeys _)
        {
            return "User";
        }

        /// <summary>
        /// 商城管理
        /// </summary>
        /// <param name="pik"><see cref="PermissionItemKeys"/></param>
        /// <returns></returns>
        public static string Shop(this PermissionItemKeys _)
        {
            return "Shop";
        }
    }
}
