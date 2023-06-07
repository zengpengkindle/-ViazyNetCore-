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

        public static ICachingBuilder UseDistributedMemoryCache(this ICachingBuilder builder, Action<DistributedCacheOptions>? options = null)
        {
            return builder.UseDistributedCache<MemoryDistributedHashCache>(options);
        }

        public static ICachingBuilder UseDistributedCache<T>(this ICachingBuilder builder, Action<DistributedCacheOptions>? options = null) where T : class, IDistributedHashCache
        {
            var option = new DistributedCacheOptions();
            options?.Invoke(option);

            builder.UseDistributedCache<T>(provider =>
            new DefaultCacheService(
                new RuntimeDistributedCacheCache(provider.GetRequiredService<IDistributedCache>()!),
                provider.GetRequiredService<RuntimeMemoryCache>(),
                option.CacheExpirationFactor,
                option.EnableDistributedCache));
            return builder;
        }

        public static ICachingBuilder UseDistributedCache<T>(this ICachingBuilder builder, Func<IServiceProvider, ICacheService> factory) where T : class, IDistributedHashCache
        {
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSingleton<IDistributedCache, T>();
            builder.Services.AddSingleton<IDistributedHashCache, T>();
            builder.Services.Replace(ServiceDescriptor.Singleton(sp => factory(sp)));
            return builder;
        }
    }
}
