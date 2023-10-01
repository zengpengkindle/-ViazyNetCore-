using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Filter;
using ViazyNetCore.Modules;
using ViazyNetCore.Swagger;

namespace ViazyNetCore.Authorization
{
    [DependsOn(typeof(EventBusModule),
        typeof(ApiManagerModule))]
    public class AuthorizationModule : InjectionModule
    {
        public AuthorizationModule()
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
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
