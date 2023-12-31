﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Identity
{
    [DependsOn(typeof(IdentityModule))]
    public class IdentityAuthModule : InjectionModule
    {
        public IdentityAuthModule()
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<IdentityOptions>(options =>
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

            Configure<AuthenticationOptions>(o =>
                {
                    o.DefaultScheme = o.DefaultScheme ?? IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = o.DefaultSignInScheme ?? IdentityConstants.ExternalScheme;
                });

            context.Services
               .AddAuthentication();

            context.Services.AddIdentity()
            .AddPasswordValidator()
            .AddErrorDescriber<CustomIdentityErrorDescriber>();
        }
    }
}
