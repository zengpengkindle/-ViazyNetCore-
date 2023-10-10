using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Filter;
using ViazyNetCore.Identity;
using ViazyNetCore.Identity.Domain;
using ViazyNetCore.Modules;
using ViazyNetCore.Swagger;

namespace ViazyNetCore.Authorization
{
    [DependsOn(typeof(EventBusModule),
        typeof(ApiManagerModule),
        typeof(IdentityAuthModule))]
    public class AuthorizationModule : InjectionModule
    {
        public AuthorizationModule()
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //context.Services.TryAddScoped<PhoneNumberTokenProvider<Identity.Domain.IdentityUser>>();
            //Configure<IdentityOptions>(options =>
            //{
            //    options.Tokens.ProviderMap.AddIfNotContains(new KeyValuePair<string, TokenProviderDescriptor>(IdentityUserManager.ResetPasswordTokenPurpose,
            //        new TokenProviderDescriptor(typeof(PhoneNumberTokenProvider<Identity.Domain.IdentityUser>))));
            //});

            Configure<AutoMapperOptions>(options => options.AddMaps<AuthorizationModule>());
            Configure<MvcOptions>(options =>
            {
                options.Filters.Add<PermissionFilter>();
            });
            Configure<SwaggerConfig>(options =>
            {
                options.Projects.Add(new ProjectConfig
                {
                    Code = "admin",
                    Description = "后台管理",
                    Name = "ViazyNetCore",
                    Version = "v2.0",
                });
            });
            context.Services.AddDynamicController(options =>
            {
                options.AddAssemblyOptions(typeof(DynamicControllerBase).Assembly);
            });
            context.Services.RegisterEventHanldersDependencies(new[] { typeof(AuthorizationModule).Assembly }, ServiceLifetime.Scoped);

            context.Services.AddTransient<IPrimssionKeyDataSeed, PrimssionKeyDataSeed>();
        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            base.PreConfigureServices(context);
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            context.ServiceProvider.UseEventBusWithStore(new[] { typeof(AuthorizationModule).Assembly });
        }
    }
}
