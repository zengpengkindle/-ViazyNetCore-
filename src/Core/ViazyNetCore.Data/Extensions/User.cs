using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ViazyNetCore.Auth;

namespace ViazyNetCore.Data.FreeSql.Extensions
{
    public class User : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public User(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        private AuthUser? AuthUser => this._accessor.GetAuthUser();
        public string Id
        {
            get
            {
                return this.AuthUser?.Id;
            }
        }
        public string Username { get => this.AuthUser?.Username; }
        public string Nickname { get => this.AuthUser?.Nickname; }

        public int IdentityType => this.AuthUser?.IdentityType ?? 0;

        public bool IsModerated => this.AuthUser?.IsModerated ?? false;
    }
}
