﻿using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Domain
{
    /// <summary>
    /// 表示一个用户的模型基类。
    /// </summary>
    public abstract class UserModelBase
    {
        /// <summary>
        /// 设置或获取一个值，表示编号。
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示用户账号。
        /// </summary>
        [MaxLength(50), Required]
        public string Username { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示用户昵称。
        /// </summary>
        [MaxLength(50), Required]
        public string Nickname { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（0禁用，1启用，-1删除）。
        /// </summary>
        [Required]
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示角色编号，为“ADMIN”表示超级管理员。
        /// </summary>
        [ Required]
        public long RoleId { get; set; }
    }
}