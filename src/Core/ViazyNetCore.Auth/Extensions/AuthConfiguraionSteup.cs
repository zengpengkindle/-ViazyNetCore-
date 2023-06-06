using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
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
            services.Configure<JwtOption>(jwtOptions);

            services.AddSingleton<TokenProvider>();
            services.AddSingleton<CustomJwtBearerEvents>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
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

                options.EventsType = typeof(CustomJwtBearerEvents);
            });
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOption>(configuration);
            var jwtOption = configuration.Get<JwtOption>();

            services.AddSingleton<TokenProvider>();
            services.AddSingleton<CustomJwtBearerEvents>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
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

                options.EventsType = typeof(CustomJwtBearerEvents);
            });
        }
    }
}
