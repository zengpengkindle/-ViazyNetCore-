using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore.Authorization.Repositories;
using ViazyNetCore.Identity.Domain.User.Repositories;
using ViazyNetCore.Modules;
using ViazyNetCore.OpenIddict.Domain;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityCoreSteup
    {
        public static void AddIdentityService(this IServiceCollection services)
        {
            services.TryAddScoped<IdentityUserClaimRepository>();
            services.TryAddScoped<IUserRepository, UserRepository>();
            services.TryAddScoped<IUserService, UserService>();
            services.TryAddScoped<IOrgRepository, OrgRepository>();
            services.TryAddScoped<IUserOrgRepository, UserOrgRepository>();
            services.TryAddScoped<IOrgService, OrgService>();
        }

        public static IdentityBuilder AddOpenIddictIdentity(this IServiceCollection services, Action<IdentityOptions> setupAction)
        {
            services.AddIdentityService();

            services.TryAddScoped<IdentityUserManager>();
            services.TryAddScoped(typeof(UserManager<ViazyNetCore.OpenIddict.Domain.IdentityUser>), provider => provider.GetService(typeof(IdentityUserManager)));

            //services.TryAddScoped<SignInManager>();
            //services.TryAddScoped(typeof(SignInManager<ViazyNetCore.OpenIddict.Domain.IdentityUser>), provider => provider.GetService(typeof(SignInManager)));

            //
            services.TryAddScoped<IdentityUserStore>();
            services.TryAddScoped(typeof(IUserStore<ViazyNetCore.OpenIddict.Domain.IdentityUser>), provider => provider.GetService(typeof(IdentityUserStore)));

            services.Configure<ViazyIdentityOptions>(options =>
            {
                options.ExternalLoginProviders.Add<ViazyExternalLoginProvider>(ViazyExternalLoginProvider.Name);
            });

            return services
                .AddIdentityCore<ViazyNetCore.OpenIddict.Domain.IdentityUser>(setupAction)
                .AddSignInManager<SignInManager>();
        }
    }
}
