using System;
using System.Collections.Generic;

namespace ViazyNetCore
{
    /// <summary>
    /// 表示一个后台管理系统的唯一标识。
    /// </summary>
    public class BmsIdentity 
    {
        /// <summary>
        /// 设置或获取一个值，表示编号。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示用户账号。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示用户昵称。
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 获取一个值，表示当前标识是否为超级管理员。
        /// </summary>
        public bool IsAdmin => this.RoleId == Globals.ADMIN_ROLE_ID;

        /// <summary>
        /// 设置或获取一个值，表示角色编号，为“ADMIN”表示超级管理员。
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示角色名称。
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示角色是否已绑定谷歌验证码
        /// </summary>
        public bool BindGoogleAuth { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示 用户必须修改密码
        /// </summary>
        public bool ResetPassword { get; set; }

        public List<string> Permissions { get; set; }
    }
}
