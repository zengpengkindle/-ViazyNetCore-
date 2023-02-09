using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ViazyNetCore.Auth.Jwt;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthConfiguraionSteup
    {
        public static void AddJwtAuthentication(this IServiceCollection services,Action<JwtOption> jwtOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (jwtOptions == null)
            {
                throw new ArgumentNullException(nameof(jwtOptions));
            }
            services.Configure(jwtOptions);
            services.AddSingleton<TokenProvider>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var provider = services.BuildServiceProvider();
                var tokenProvider = provider.GetRequiredService<TokenProvider>();
                var jwtConfig = provider.GetRequiredService<IOptions<JwtOption>>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Value.Secret)),
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireAudience = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };

                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        if (context.SecurityToken is JwtSecurityToken securityToken)
                            return tokenProvider.ValidToken(securityToken);
                        else
                            throw new UnauthorizedAccessException();
                    }

                };
            });
        }
    }
}
