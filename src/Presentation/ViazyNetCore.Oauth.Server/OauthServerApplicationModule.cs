using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIddict.Validation.AspNetCore;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.Authorization;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.Identity;
using ViazyNetCore.OpenIddict;

namespace ViazyNetCore.Oauth.Server
{
    [DependsOn(typeof(AutoMapperModule),
        typeof(OpenIddictModule),
        typeof(AspNetCoreModule),
        typeof(AuthorizationModule))]
    public class OauthServerApplicationModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAuthentication(options =>
             {
                 //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                 // options.RequireAuthenticatedSignIn = true;

             });

            context.Services.AddIdentity(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.User.RequireUniqueEmail = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
        }
    }
}
