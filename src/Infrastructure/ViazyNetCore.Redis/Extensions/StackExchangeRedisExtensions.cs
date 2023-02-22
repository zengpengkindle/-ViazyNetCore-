using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using ViazyNetCore.Caching;
using ViazyNetCore.Configuration;
using ViazyNetCore.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StackExchangeRedisExtensions
    {
        public static void AddRedisDistributedHashCache(this IServiceCollection services, Action<RedisCacheOptions> setupAction)
        {

            if (services == null)
            {
                throw new ArgumentNullException("services");
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException("setupAction");
            }

            services.AddOptions();
            services.Configure(setupAction);

            services.AddSingleton(sp =>
            {
                var options = sp.GetService<IOptions<RedisCacheOptions>>()!.Value;
                IConnectionMultiplexer connection;
                if (options.ConnectionMultiplexerFactory == null)
                {
                    if (options.ConfigurationOptions != null)
                    {
                        connection = ConnectionMultiplexer.Connect(options.ConfigurationOptions);
                    }
                    else
                    {
                        connection = ConnectionMultiplexer.Connect(options.Configuration);
                    }
                }
                else
                {
                    connection = options.ConnectionMultiplexerFactory!().GetAwaiter().GetResult();
                }

                return connection;
            });
            services.AddSingleton<IRedisCache, RedisService>();
            services.AddSingleton<IDistributedCache, RedisDistributedHashCache>();
            services.AddSingleton<IDistributedHashCache, RedisDistributedHashCache>();
        }

        public static void AddRedisCacheSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IRedisCache, RedisService>();

            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton(sp =>
            {
                //获取连接字符串
                string redisConfiguration = AppSettingsConstVars.RedisConfigConnectionString;

                var configuration = ConfigurationOptions.Parse(redisConfiguration, true);

                configuration.ResolveDns = true;

                return ConnectionMultiplexer.Connect(configuration);
            });

        }
    }
}
