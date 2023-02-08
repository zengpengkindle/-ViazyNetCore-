using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IEventBusServiceCollection
    {
        IServiceCollection Services { get; }
    }

    public static class EventBusExtensions
    {
        private class EventBusServiceCollection : IEventBusServiceCollection
        {

            public EventBusServiceCollection(IServiceCollection services)
            {
                this.Services = services;
            }

            public IServiceCollection Services { get; }
        }

        /// <summary>
        /// 添加事件总线。
        /// </summary>
        /// <param name="services">服务提供程序。</param>
        /// <returns>服务集合。</returns>
        public static IEventBusServiceCollection AddEventBus(this IServiceCollection services)
        {
            if(services is null)
                throw new ArgumentNullException(nameof(services));
            services.AddSingleton<IEventStore, MemoryEventStore>();
            services.AddScoped<IEventBus, EventBus>();

            return new EventBusServiceCollection(services);
        }

        public static IServiceCollection RegisterEventHanldersDependencies(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            var assemblies = RuntimeHelper.GetAllAssemblies().ToArray();

            RegisterEventHanldersDependencies(services, assemblies, serviceLifetime);

            return services;
        }

        public static IServiceCollection RegisterEventHanldersDependencies(this IServiceCollection services, Assembly?[] assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            foreach(var assembly in assemblies)
            {
                if (assembly == null) continue;
                foreach(var t in assembly.DefinedTypes)
                {
                    if(t.IsAbstract) continue;
                    AddEventHanlder(services, t, serviceLifetime);
                }
            }

            return services;
        }

        private static void AddEventHanlder(IServiceCollection services, TypeInfo type, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            if(type.IsAbstract) return;
            var serviceType = type.GetInterface($"I{type.Name}");
            var attr = (serviceType ?? type).GetAttribute<InjectionHanlderAttribute>();

            if(attr is null) return;

            var handlerInterface = type.GetInterface("IEventHandler`1");
            var handlerInterfaceAsync = type.GetInterface("IEventHandlerAsync`1");
            if(handlerInterface == null && handlerInterfaceAsync == null) return;

            if(handlerInterface != null)
                services.TryAdd(ServiceDescriptor.Describe(handlerInterface, type, serviceLifetime));

            if(handlerInterfaceAsync != null)
                services.TryAdd(ServiceDescriptor.Describe(handlerInterfaceAsync, type, serviceLifetime));
        }

        public static IApplicationBuilder UseEventBusWithStore(this IApplicationBuilder builder, Assembly[] assemblies)
        {
            var services = builder.ApplicationServices;
            var eventStore = services.GetService<IEventStore>();
            if(eventStore is null)
                throw new InvalidOperationException("Can't found EventBus.");
            foreach(var assembly in assemblies)
            {
                eventStore.RegisterAllEventHandlerFromAssembly(assembly);
            }

            return builder;
        }
    }
}
