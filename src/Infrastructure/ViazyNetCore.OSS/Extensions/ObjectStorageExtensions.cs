using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore.OSS;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ObjectStorageExtensions
    {
        /// <summary>
        /// 从配置文件中加载默认配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IServiceCollection AddOSSService(this IServiceCollection services, string key)
        {
            return services.AddOSSService(DefaultOptionName.Name, key);
        }

        /// <summary>
        /// 从配置文件中加载
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IServiceCollection AddOSSService(this IServiceCollection services, string name, string key)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(IConfiguration));
            }
            provider.Dispose();

            IConfigurationSection section = configuration.GetSection(key);
            if (!section.Exists())
            {
                throw new Exception($"Config file not exist '{key}' section.");
            }
            OSSOptions options = section.Get<OSSOptions>();
            if (options == null)
            {
                throw new Exception($"Get OSS option from config file failed.");
            }
            return services.AddOSSService(name, o =>
            {
                o.AccessKey = options.AccessKey;
                o.Endpoint = options.Endpoint;
                o.IsEnableCache = options.IsEnableCache;
                o.IsEnableHttps = options.IsEnableHttps;
                o.Provider = options.Provider;
                o.Region = options.Region;
                o.SecretKey = options.SecretKey;
                o.SessionToken = options.SessionToken;
            });
        }

        /// <summary>
        /// 配置默认配置
        /// </summary>
        public static IServiceCollection AddOSSService(this IServiceCollection services, Action<OSSOptions> option)
        {
            return services.AddOSSService(DefaultOptionName.Name, option);
        }

        /// <summary>
        /// 根据名称配置
        /// </summary>
        public static IServiceCollection AddOSSService(this IServiceCollection services, string name, Action<OSSOptions> option)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = DefaultOptionName.Name;
            }
            services.Configure(name, option);

            return services;
        }
    }
}
