using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using ViazyNetCore.EventBus.Distributed;
using ViazyNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DistributedEventBusExtensions
    {

        public static IServiceCollection RegisterDistributedEventHanldersDependencies(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            var assemblies = RuntimeHelper.GetAllAssemblies().ToArray();

            RegisterDistributedEventHanldersDependencies(services, assemblies, serviceLifetime);

            return services;
        }

        public static IServiceCollection RegisterDistributedEventHanldersDependencies(this IServiceCollection services, Assembly?[] assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            foreach (var assembly in assemblies)
            {
                if (assembly == null) continue;
                foreach (var t in assembly.DefinedTypes)
                {
                    if (t.IsAbstract) continue;
                    AddDistributedEventHanlder(services, t, serviceLifetime);
                }
            }

            return services;
        }

        private static void AddDistributedEventHanlder(IServiceCollection services, TypeInfo type, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            if (type.IsAbstract) return;
            var serviceType = type.GetInterface($"I{type.Name}");
            var attr = (serviceType ?? type).GetAttribute<InjectionHanlderAttribute>();

            if (attr is null) return;

            var handlerInterfaceAsync = type.GetInterface("IDistributedEventHandler`1");
            if (handlerInterfaceAsync == null) return;

            if (handlerInterfaceAsync != null)
            {
                services.TryAdd(ServiceDescriptor.Describe(handlerInterfaceAsync, type, serviceLifetime));
                services.TryAdd(ServiceDescriptor.Describe(type, type, serviceLifetime));
            }
        }

        //public static IApplicationBuilder UseDistributedEventBusWithStore(this IApplicationBuilder builder, Assembly[] assemblies)
        //{
        //    var services = builder.ApplicationServices;
        //    var eventStore = services.GetService<IDistributedEventStore>();
        //    if (eventStore is null)
        //        throw new InvalidOperationException("Can't found EventBus.");
        //    foreach (var assembly in assemblies)
        //    {
        //        eventStore.RegisterAllEventHandlerFromAssembly(assembly);
        //    }

        //    return builder;
        //}

        //public static void UseDistributedEventBusWithStore(this IServiceProvider serviceProvider, Assembly[] assemblies)
        //{
        //    var eventStore = serviceProvider.GetService<IDistributedEventStore>();
        //    if (eventStore is null)
        //        throw new InvalidOperationException("Can't found EventBus.");
        //    foreach (var assembly in assemblies)
        //    {
        //        eventStore.RegisterAllEventHandlerFromAssembly(assembly);
        //    }
        //}
    }
}
