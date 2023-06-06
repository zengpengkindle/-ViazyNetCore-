using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore.Caching;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ICachingBuilder AddCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<RuntimeMemoryCache>();
            services.AddSingleton(typeof(ICacheService), sp =>
            {
                var localCache = sp.GetService<RuntimeMemoryCache>()!;
                return new DefaultCacheService(localCache, localCache, 1, false);
            });

            var builder = new DefaultCachingBuilder(services);
            return builder;
        }

        public static ICachingBuilder UseDistributedMemoryCache(this ICachingBuilder builder)
        {
            return builder.UseDistributedCache<MemoryDistributedHashCache>();
        }

        public static ICachingBuilder UseDistributedCache<T>(this ICachingBuilder builder) where T : class, IDistributedHashCache
        {
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSingleton<IDistributedCache, T>();
            builder.Services.AddSingleton<IDistributedHashCache, T>();
            builder.Services.Replace(ServiceDescriptor.Singleton<ICacheService>(sp =>
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
            return builder;
        }
    }
}
