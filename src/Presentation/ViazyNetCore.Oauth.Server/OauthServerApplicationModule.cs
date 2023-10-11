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
        typeof(IdentityAuthModule),
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
        }
    }
}
