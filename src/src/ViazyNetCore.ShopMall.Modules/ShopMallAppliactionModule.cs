﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Modules
{
    [DependsOn(typeof(EventBusModule))]
    public class ShopMallAppliactionModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<ShopMallAppliactionModule>());
            context.Services.AddEventBus();
            context.Services.RegisterEventHanldersDependencies(new[] { typeof(ShopMallAppliactionModule).Assembly }, ServiceLifetime.Scoped);
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            context.ServiceProvider.UseEventBusWithStore(new[] { typeof(ShopMallAppliactionModule).Assembly });
        }
    }
}
