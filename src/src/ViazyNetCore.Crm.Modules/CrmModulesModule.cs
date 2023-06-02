using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Modules
{
    [DependsOn(typeof(EventBusModule))]
    public class CrmModulesModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<CrmModulesModule>());
            context.Services.RegisterEventHanldersDependencies(new[] { typeof(CrmModulesModule).Assembly }, ServiceLifetime.Scoped);
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            context.ServiceProvider.UseEventBusWithStore(new[] { typeof(CrmModulesModule).Assembly });
        }
    }
}
