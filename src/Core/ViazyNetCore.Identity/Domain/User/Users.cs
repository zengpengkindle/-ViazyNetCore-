using ViazyNetCore.Authorization.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.Authorization.Models
{
    public partial class BmsUser : IUser
    {
        public DateTime ModifyTime { get; set; }

        public string? ExtraData { get; set; }

        [Column(IsIgnore = true)]
        public bool IsModerated => false;

        public string GoogleKey { get; internal set; }

        [Column(IsIgnore = true)]
        public AuthUserType IdentityType { get; set; } = AuthUserType.Normal;

        public long TenantId { get; set; }
    }
}
