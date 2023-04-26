using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViazyNetCore.Authorization.Model;

namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 表示一个管理用户。
    /// </summary>
    public partial class BmsUser : EntityAdd<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示账号。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示加盐值。
        /// </summary>
        public Guid PasswordSalt { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示昵称。
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（-1删除，0禁用，1启用）。
        /// </summary>
        public ComStatus Status { get; set; }

    }
}
