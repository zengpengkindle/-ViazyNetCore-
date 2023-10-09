using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore.Authorization.Repositories;
using ViazyNetCore.Identity;
using ViazyNetCore.Identity.Domain;
using ViazyNetCore.Identity.Domain.Models;
using ViazyNetCore.Identity.Domain.User.Repositories;
using ViazyNetCore.Identity.Validator;
using ViazyNetCore.Modules;

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

        public static IdentityBuilder AddIdentity(this IServiceCollection services)
        {
            //services.AddIdentityService();

            services.TryAddScoped<IdentityUserManager>();
            services.TryAddScoped(typeof(UserManager<IdentityUser>), provider => provider.GetService(typeof(IdentityUserManager))!);

            services.TryAddScoped<SignInManager>();
            services.TryAddScoped(typeof(SignInManager<IdentityUser>), provider => provider.GetService(typeof(SignInManager))!);

            //
            services.TryAddScoped<IdentityUserStore>();
            services.TryAddScoped(typeof(IUserStore<IdentityUser>), provider => provider.GetService(typeof(IdentityUserStore))!);

            services.TryAddScoped<RoleStore>();
            services.TryAddScoped(typeof(IRoleStore<BmsRole>), provider => provider.GetService(typeof(RoleStore))!);

            services.Configure<ViazyIdentityOptions>(options =>
            {
                //options.ExternalLoginProviders.Add<ViazyExternalLoginProvider>(ViazyExternalLoginProvider.Name);
            });

            return services
                .AddIdentityCore<IdentityUser>()
                .AddRoles<BmsRole>()
                .AddSignInManager<SignInManager>()
                .AddUserManager<IdentityUserManager>()
                .AddRoleManager<RoleManager>()
                .AddDefaultTokenProviders()
                ;
        }

        public static IdentityBuilder AddPasswordValidator(this IdentityBuilder builder)
        {
            builder.Services.AddScoped<IPasswordHasher<IdentityUser>, PasswordHasher>();

            builder.Services.AddScoped<ViazySecurityStampValidator>();
            builder.Services.AddScoped(typeof(SecurityStampValidator<IdentityUser>)
                , provider => provider.GetService(typeof(ViazySecurityStampValidator))!);

            return builder.AddPasswordValidator<PasswordValidator>();
        }
    }
}
