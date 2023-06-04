using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ViazyNetCore.Auth.Jwt
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly JwtOption _option;
        private readonly TokenProvider _tokenProvider;

        public CustomJwtBearerEvents(JwtOption option, TokenProvider tokenProvider)
        {
            this._option = option;
            this._tokenProvider = tokenProvider;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            //return base.TokenValidated(context);
            await this._tokenProvider.ValidToken(context.SecurityToken as JwtSecurityToken);
        }
    }
}
