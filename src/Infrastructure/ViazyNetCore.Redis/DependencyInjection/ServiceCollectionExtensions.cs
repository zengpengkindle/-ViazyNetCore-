using System;
using System.Collections.Generic;
using System.Linq;
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
        public static ICachingBuilder UseStackExchangeRedisCaching(this ICachingBuilder services, Action<RedisCacheOptions> setupAction)
        {
            services.Services.AddStackExchangeRedisCache(setupAction);
            services.Services.AddSingleton<IRedisCache, RedisService>();
            services.Services.Replace(ServiceDescriptor.Singleton(typeof(IDistributedCache), typeof(RedisDistributedHashCache)));
            services.Services.Replace(ServiceDescriptor.Singleton(typeof(IDistributedHashCache), typeof(RedisDistributedHashCache)));
            services.Services.Replace(ServiceDescriptor.Singleton<ICacheService>(sp =>
            {
                var localCache = sp.GetService<RuntimeMemoryCache>()!;
                var distributedCache = sp.GetService<IDistributedCache>();
                bool enableDistributedCache = false;
                ICache cache;
                if (distributedCache != null)
                {
                    enableDistributedCache = true;
                    cache = new RuntimeDistributedCacheCache(distributedCache);
                }
                else
                {
                    cache = localCache;
                }
                return new DefaultCacheService(cache, localCache, 1, enableDistributedCache);
            }));
            return services;
        }
    }
}
