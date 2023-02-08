using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个平台用户。
    /// </summary>
    public partial class BmsUser : EntityBase
    {
        /// <summary>
        /// 设置或获取一个值，表示用户账号。
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示用户密码。
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示用户密码加盐值。
        /// </summary>
        public Guid PasswordSalt { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示会员昵称。
        /// </summary>
        [Required]
        public string Nickname { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（0禁用，1启用，-1删除）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示角色编号，为“ADMIN”表示超级管理员。
        /// </summary>
        [Required]
        public long RoleId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示修改时间。
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string ExtraData { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示谷歌验证器密钥。
        /// </summary>
        [JsonIgnore]
        public string GoogleKey { get; set; }

    }
}
