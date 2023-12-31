﻿using System;
using System.Text.Json.Serialization;
using ViazyNetCore.CmsKit;

namespace ViazyNetCore.CmsKit.Dtos
{
    /// <summary>
    /// 表示一个用户信息。
    /// </summary>
    public partial class UserDto
    {
        /// <summary>
        /// 设置或获取一个值，表示编号
        /// </summary>
        public long Id { get; set; }

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
        /// 设置或获取一个值，表示状态。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string? GoogleKey { get; set; }

        public string? ExtendData { get; set; }

        public string? RoleExtendData { get; set; }
    }
}
