using System.Diagnostics.CodeAnalysis;
using System.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.AspNetCore.Core;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.Identity;
using ViazyNetCore.Manage.WebApi.Middlewares;
using ViazyNetCore.Modules;
using ViazyNetCore.MultiTenancy;
using ViazyNetCore.ShopMall.Manage.Application;
using ViazyNetCore.Swagger;

namespace ViazyNetCore.Manage.WebApi
{
    [DependsOn(typeof(AutoMapperModule)
        , typeof(AspNetCoreMvcModule)
        , typeof(AuthorizationModule)
        , typeof(IdentityModule)
        , typeof(ShopMallManageModule)
        //, typeof(RabbitMQEventBusModule)
        , typeof(MultiTenancyModule)
        , typeof(EventBusModule)
        )]
    public class BmsApplicationModule : InjectionModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AppOptions>(options =>
            {
                options.ApplicationParts.Add(typeof(AuthorizationModule).Assembly);
                options.ApplicationParts.Add(typeof(ShopMallManageModule).Assembly);
                //options.ApplicationParts.Add(typeof(JobSetup).Assembly);
            });
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<BmsApplicationModule>());

            //context.Services.AddMQueue();
            //context.Services.AddJobSetup();
            //context.Services.AddJobTaskSetup();

            Configure<SwaggerConfig>(options =>
            {
                options.Projects.Add(new ProjectConfig
                {
                    Code = "task",
                    Description = "TaskJob",
                    Name = "TaskJob",
                    Version = "v1",
                });

                options.Projects.Add(new ProjectConfig
                {
                    Code = "shopmall",
                    Description = "ShopMall",
                    Name = "ShopMall",
                    Version = "v1",
                });
            });
            context.Services.AddSwagger();
            //context.Services.AddSingleton<TunnelWorksMultiTenancyMiddleware>();
            //context.Services.RegisterDistributedEventHanldersDependencies(new[] { typeof(BmsApplicationModule).Assembly }, ServiceLifetime.Scoped);
        }
        public override async Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            var dataSeed = app.ApplicationServices.GetService<IPrimssionKeyDataSeed>();
            await dataSeed.CreatePrimissionKeyAsync();
            //app.UseMiddleware<TunnelWorksMultiTenancyMiddleware>();
        }
    }
}
