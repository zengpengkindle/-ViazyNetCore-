using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Modules
{
    public class BmsIdentity : IUser
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public AuthUserType IdentityType { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string UserType => this.IdentityType.ToString();

        public bool IsModerated => false;

        public bool BindGoogleAuth { get; internal set; }
        public List<string> Permissions { get; internal set; }
    }
}
