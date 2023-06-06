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
            Check.NotNull(services, nameof(IServiceCollection));
            Check.NotNull(options, nameof(RedisCacheOptions));

            services.AddOptions();
            services.Configure(options);

            services.AddSingleton<IRedisCache, RedisService>();
            services.AddSingleton<IDistributedCache, RedisDistributedHashCache>();
            services.AddSingleton<IDistributedHashCache, RedisDistributedHashCache>();
        }

        public static void AddRedisCacheSetup(this IServiceCollection services, Action<RedisCacheOptions> options)
        {
            Check.NotNull(services, nameof(IServiceCollection));

            services.Configure(options);
            services.AddSingleton<IRedisCache, RedisService>();

        }
    }
}
