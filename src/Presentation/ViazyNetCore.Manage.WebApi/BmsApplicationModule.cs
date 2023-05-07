using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Newtonsoft.Json;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.DI;
using ViazyNetCore.Identity;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Manage.WebApi
{
    [DependsOn(typeof(AutoMapperModule)
        , typeof(AspNetCoreMvcModule)
        , typeof(AuthorizationModule)
        , typeof(IdentityModule)
        , typeof(ShopMallAppliactionModule)
        )]
    public class BmsApplicationModule : InjectionModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AppOptions>(options =>
            {
                options.ApplicationParts.Add(typeof(AuthorizationModule).Assembly);
                options.ApplicationParts.Add(typeof(JobSetup).Assembly);
            });
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMQueue();
            context.Services.AddJobSetup();
            context.Services.AddJobTaskSetup();
        }
        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
        }
    }
}
