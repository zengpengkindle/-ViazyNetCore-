using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Caching.DependencyInjection
{
    public static class RumtimeCacheDependencyExtenstions
    {

        /// <summary>
        /// 依赖注入指定程序集所有指定 <see cref="InjectionAttribute"/> 特征的类型。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <param name="assemblies">程序集的列表。</param>
        /// <returns>服务集合。</returns>
        public static IServiceCollection AddRumtimeCacheService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<RuntimeMemoryCache>();
            services.AddSingleton(typeof(ICacheService), sp =>
            {
                var localCache = sp.GetService<RuntimeMemoryCache>()!;
                var distributedCache = sp.GetService<IDistributedCache>();
                bool enableDistributedCache = false;
                ICache cache;
                if(distributedCache != null)
                {
                    enableDistributedCache = true;
                    cache = new RuntimeDistributedCacheCache(distributedCache);
                }
                else
                {
                    cache = localCache;
                }
                return new DefaultCacheService(cache, localCache, 1, enableDistributedCache);
            });

            return services;
        }

        /// <summary>
        /// 依赖注入指定程序集所有指定 <see cref="InjectionAttribute"/> 特征的类型。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <param name="assemblies">程序集的列表。</param>
        /// <returns>服务集合。</returns>
        public static IServiceCollection AddLocalCacheService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<RuntimeMemoryCache>();
            services.AddSingleton(typeof(ICacheService), sp =>
            {
                var localCache = sp.GetService<RuntimeMemoryCache>();
                var distributedCache = sp.GetService<IDistributedCache>();
                if(localCache == null)
                    throw new ArgumentNullException(nameof(localCache));
                if(distributedCache == null)
                    throw new ArgumentNullException(nameof(IDistributedCache));
                
                ICache cache;
                cache = new RuntimeDistributedCacheCache(distributedCache);
                return new DefaultCacheService(localCache, localCache, 1, false);
            });

            return services;
        }
    }
}