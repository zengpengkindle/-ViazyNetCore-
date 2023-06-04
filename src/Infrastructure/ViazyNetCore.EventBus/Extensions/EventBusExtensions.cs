using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore;
using ViazyNetCore.EventBus.Distributed;

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
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            services.TryAddSingleton<ILocalEventStore, LocalEventStore>();
            services.TryAddSingleton<IEventHandlerInvoker, EventHandlerInvoker>();
            services.TryAddScoped<IEventBus, DefaultEventBus>();

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
            foreach (var assembly in assemblies)
            {
                if (assembly == null) continue;
                foreach (var t in assembly.DefinedTypes)
                {
                    if (t.IsAbstract) continue;
                    AddEventHanlder(services, t, serviceLifetime);
                }
            }

            return services;
        }

        private static void AddEventHanlder(IServiceCollection services, TypeInfo type, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            if (type.IsAbstract) return;
            var serviceType = type.GetInterface($"I{type.Name}");
            var attr = (serviceType ?? type).GetAttribute<InjectionHanlderAttribute>();

            if (attr is null) return;

            var handlerInterfaceAsync = type.GetInterface("ILocalEventHandler`1");
            if (handlerInterfaceAsync == null) return;

            if (handlerInterfaceAsync != null)
            {
                services.TryAdd(ServiceDescriptor.Describe(handlerInterfaceAsync, type, serviceLifetime));
                services.TryAdd(ServiceDescriptor.Describe(type, type, serviceLifetime));
            }
        }

        public static IApplicationBuilder UseEventBusWithStore(this IApplicationBuilder builder, Assembly[] assemblies)
        {
            var services = builder.ApplicationServices;
            var eventStore = services.GetService<ILocalEventStore>();
            if (eventStore is null)
                throw new InvalidOperationException("Can't found EventBus.");
            foreach (var assembly in assemblies)
            {
                eventStore.RegisterAllEventHandlerFromAssembly(assembly);
            }

            return builder;
        }

        public static void UseEventBusWithStore(this IServiceProvider serviceProvider, Assembly[] assemblies)
        {
            var eventStore = serviceProvider.GetService<ILocalEventStore>();
            if (eventStore is null)
                throw new InvalidOperationException("Can't found EventBus.");
            foreach (var assembly in assemblies)
            {
                eventStore.RegisterAllEventHandlerFromAssembly(assembly);
            }
        }
    }
}
