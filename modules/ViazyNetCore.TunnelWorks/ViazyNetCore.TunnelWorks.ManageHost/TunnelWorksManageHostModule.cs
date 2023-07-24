using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.Auth;
using ViazyNetCore.Authorization;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.Data.FreeSql.Extensions;
using ViazyNetCore.Identity;
using ViazyNetCore.Modules;
using ViazyNetCore.MultiTenancy;
using ViazyNetCore.Swagger;
using ViazyNetCore.TunnelWorks.ManageHost.Middlewares;

namespace ViazyNetCore.TunnelWorks.ManageHost
{
    [DependsOn(typeof(AutoMapperModule)
        , typeof(IdentityModule)
        , typeof(AspNetCoreMvcModule)
        , typeof(AuthorizationModule)
        , typeof(AuthApplicationModule)
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
            });
            context.Services.AddSwagger();
            //context.Services.AddScoped<TunnelWorksMultiTenancyMiddleware>();
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            //app.UseMiddleware<TunnelWorksMultiTenancyMiddleware>();
        }
    }
}
