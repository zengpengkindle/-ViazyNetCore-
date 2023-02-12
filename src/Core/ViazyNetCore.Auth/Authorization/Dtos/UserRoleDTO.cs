using System;
using System.Text.Json.Serialization;
using ViazyNetCore.Authorization;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示一个三方银行信息。
    /// </summary>
    public partial class UserRoleDTO 
    {
        /// <summary>
        /// 设置或获取一个值，表示编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示用户名称。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示密码加密盐。
        /// </summary>
        public Guid PasswordSalt { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示昵称。
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示角色编号。
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示角色名称。
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 设置或获取一个值，表示状态。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string GoogleKey { get; set; }

        public string ExtendData { get; set; }

        public string RoleExtendData { get; set; }
    }
}
