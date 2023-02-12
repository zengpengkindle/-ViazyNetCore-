using ViazyNetCore.Authorization.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViazyNetCore.Authorization.Models
{
    public partial class BmsUser : IUser
    {
        public DateTime ModifyTime { get; set; }
        public string? ExtraData { get; set; }

        public int IdentityType => 0;

        public bool IsModerated => false;

        public string GoogleKey { get; internal set; }
    }
}
