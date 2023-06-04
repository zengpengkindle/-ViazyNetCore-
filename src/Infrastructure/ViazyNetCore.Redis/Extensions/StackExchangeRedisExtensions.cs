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
        public static void AddRedisDistributedHashCache(this IServiceCollection services, Action<RedisCacheOptions> options)
        {

            if (services == null)
            {
                throw new ArgumentNullException("services");
            }

            if (options == null)
            {
                throw new ArgumentNullException("setupAction");
            }

            services.AddOptions();
            services.Configure(options);

            services.AddSingleton<IRedisCache, RedisService>();
            services.AddSingleton<IDistributedCache, RedisDistributedHashCache>();
            services.AddSingleton<IDistributedHashCache, RedisDistributedHashCache>();
        }

        public static void AddRedisCacheSetup(this IServiceCollection services, Action<RedisCacheOptions> options)
        {
            services.Configure(options);
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IRedisCache, RedisService>();

        }
    }
}
