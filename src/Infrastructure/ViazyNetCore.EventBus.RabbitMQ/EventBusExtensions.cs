using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore;
using ViazyNetCore.EventBus.Distributed;
using ViazyNetCore.EventBus.RabbitMQ;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusExtensions
    {
        /// <summary>
        /// 添加事件总线。
        /// </summary>
        /// <param name="services">服务提供程序。</param>
        /// <returns>服务集合。</returns>
        public static IEventBusServiceCollection AddRabbitMQEventBus(this IEventBusServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            services.Services.Replace(new ServiceDescriptor(typeof(IEventBus), typeof(DistributedEventBus), ServiceLifetime.Singleton));
            services.Services.TryAddSingleton<IDistributedEventStore, RabbitMqDistributedEventStore>();
            return services;
        }

        internal static void AddDistributedEventBus(this IServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            services.Replace(new ServiceDescriptor(typeof(IEventBus), typeof(DistributedEventBus), ServiceLifetime.Singleton));
            services.TryAddSingleton<IDistributedEventStore, RabbitMqDistributedEventStore>();
        }
    }
}
