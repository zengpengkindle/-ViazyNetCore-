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
        public int IdentityType => 0;
        [Column(IsIgnore =true)]
        public bool IsModerated => false;

        public string GoogleKey { get; internal set; }
    }
}
