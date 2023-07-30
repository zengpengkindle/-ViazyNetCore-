using System.Diagnostics.CodeAnalysis;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.Auth;
using ViazyNetCore.Authorization;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.Identity;
using ViazyNetCore.Modules;
using ViazyNetCore.Swagger;

namespace ViazyNetCore.TunnelWorks.ManageHost
{
    [DependsOn(typeof(AutoMapperModule)
        , typeof(IdentityModule)
        , typeof(AspNetCoreMvcModule)
        , typeof(AuthorizationModule)
        , typeof(AuthApplicationModule)
        , typeof(TunnelWorksModulsModule)
        )]
    public class TunnelWorksManageHostModule : InjectionModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AppOptions>(options =>
            {
                options.ApplicationParts.Add(typeof(AuthorizationModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<TunnelWorksManageHostModule>());

            Configure<SwaggerConfig>(options =>
            {
                options.Projects.Add(new ProjectConfig
                {
                    Name = "表单设计",
                    Code = "tunnelwork",
                    Description = "表单设计",
                    Version = "1.0",
                });
            });
            context.Services.AddSwagger();
            //context.Services.AddScoped<TunnelWorksMultiTenancyMiddleware>();
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            //var app = context.GetApplicationBuilder();

            //app.UseMiddleware<TunnelWorksMultiTenancyMiddleware>();
        }
    }
}
