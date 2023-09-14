using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore.Caching;
using ViazyNetCore.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ICachingBuilder UseStackExchangeRedisCaching(this ICachingBuilder services, Action<RedisCacheOptions> setupAction, Action<DistributedCacheOptions>? options = null)
        {
            services.Services.AddStackExchangeRedisCache(setupAction);
            services.Services.AddSingleton<IRedisCache, RedisService>();

            services.UseDistributedCache<DefaultRedisCache>(options);

            return services;
        }
    }
}
