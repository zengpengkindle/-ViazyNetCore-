using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Modules;

namespace ViazyNetCore
{
    [DependsOn(typeof(EventBusModule))]
    public class RabbitMQEventBusModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddDistributedEventBus();
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
        }
    }
}
