using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 表示一个管理用户。
    /// </summary>
    public partial class BmsUser : EntityUpdate<long>, IUser, ITenant
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
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        public string? ExtraData { get; set; }

        /// <summary>
        /// 是否被管制
        /// </summary>
        [Column(IsIgnore = true)]
        public bool IsModerated => false;

        public string GoogleKey { get; internal set; }

        [Column(IsIgnore = true, MapType = typeof(int))]
        public AuthUserType IdentityType { get; set; } = AuthUserType.Normal;

        /// <summary>
        /// 设置或获取一个值，表示状态（-1删除，0禁用，1启用）。
        /// </summary>
        [Column(MapType = typeof(int))]
        public ComStatus Status { get; set; }

        public DateTime ModifyTime { get; set; }

        public long TenantId { get; set; }
    }
}
