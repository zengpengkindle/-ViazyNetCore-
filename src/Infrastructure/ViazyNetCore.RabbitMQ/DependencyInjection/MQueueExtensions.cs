using System;

using ViazyNetCore.RabbitMQ;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 表示消息队列的启动库。
    /// </summary>
    public static class MQueueExtensions
    {
        /// <summary>
        /// 添加消息队列。
        /// </summary>
        /// <param name="services">服务容器。</param>
        /// <param name="declareSetup">声明配置。</param>
        /// <returns>服务容器。</returns>
        public static IServiceCollection AddMQueue(this IServiceCollection services, Action<IDeclareFactory>? declareSetup = null)
        {
            if(services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IMessageSerializer, JsonMessageSerializer>();
            services.TryAddSingleton<IChannelProxyPool, DefaultIChannelProxyPool>();
            services.TryAddSingleton<IMessageBus, DefaultMessageBus>();
            services.TryAddSingleton<IMessageManager>(sp => new ConfigurationMessageManager(sp.GetRequiredService<IConfiguration>()));
            services.TryAddSingleton<IMessageFactory, DefaultMessageFactory>();
            var ddf = new DefaultDeclareFactory();
            declareSetup?.Invoke(ddf);
            services.TryAddSingleton<IDeclareFactory>(ddf);
            return services;
        }
    }
}
