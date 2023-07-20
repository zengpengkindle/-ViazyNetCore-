using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ViazyNetCore.Data.FreeSql.Extensions
{
    public class User : IUser
    {
        private readonly IHttpContextAccessor _accessor;


        public User(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        public long Id
        {
            get
            {
                return this._accessor?.GetUserId() ?? 0;
            }
        }
        public string Username { get => this._accessor?.GetUserName() ?? string.Empty; }
        public string Nickname { get => this._accessor?.GetNickName() ?? string.Empty; }

        public AuthUserType IdentityType => this._accessor?.GetAuthUserType() ?? AuthUserType.Normal;

        public bool IsModerated => false;

        public long TenantId { get; set; }
    }
}
