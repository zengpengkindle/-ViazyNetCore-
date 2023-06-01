using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.Modules;

namespace ViazyNetCore.ShopMall.Manage.Application
{
    [DependsOn(typeof(AutoMapperModule)
        , typeof(ShopMallModulesModule)
        )]
    public class ShopMallManageModule : InjectionModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<ShopMallManageModule>());
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
        }
    }
}