using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ViazyNetCore.Auth.Jwt;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthConfiguraionSteup
    {
        public static void AddJwtAuthentication(this IServiceCollection services, Action<JwtOption> jwtOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (jwtOptions == null)
            {
                throw new ArgumentNullException(nameof(jwtOptions));
            }
            var jwtOption = new JwtOption();
            jwtOptions.Invoke(jwtOption);
            services.AddSingleton(jwtOption);

            services.AddSingleton<TokenProvider>();
            services.AddSingleton<CustomJwtBearerEvents>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var provider = services.BuildServiceProvider();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Secret)),
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };

                //options.Events = new JwtBearerEvents()
                //{
                //    OnTokenValidated = context =>
                //    {
                //        if (context.SecurityToken is JwtSecurityToken securityToken)
                //            return tokenProvider.ValidToken(securityToken);
                //        else
                //            throw new UnauthorizedAccessException();
                //    }

                //};
                options.EventsType = typeof(CustomJwtBearerEvents);
            });
        }
    }
}
