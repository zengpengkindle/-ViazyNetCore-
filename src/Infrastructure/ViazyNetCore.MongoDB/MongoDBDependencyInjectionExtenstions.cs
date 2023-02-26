using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Repos;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 表示 MongoDB 依赖注入扩展。
    /// </summary>
    public static class MongoDBDependencyInjectionExtenstions
    {
        /// <summary>
        /// 添加 MongoDB 管理器
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <param name="sectionName">节点名称。</param>
        /// <returns>服务集合。</returns>
        public static IServiceCollection AddRepositoryMongoDB(this IServiceCollection services, string sectionName = "MongoDB") 
            => services.AddRepositoryMongoDB<MongoDBRepositoryManager>(sectionName);

        /// <summary>
        /// 添加 MongoDB 管理器
        /// </summary>
        /// <typeparam name="TRepositoryManager">仓库管理器的数据类型。</typeparam>
        /// <param name="services">服务集合。</param>
        /// <param name="sectionName">节点名称。</param>
        /// <returns>服务集合。</returns>
        public static IServiceCollection AddRepositoryMongoDB<TRepositoryManager>(this IServiceCollection services, string sectionName = "MongoDB")
            where TRepositoryManager : class, IRepositoryManager, new()
        {
            services.AddSingleton<IRepositoryManager>(sp => sp.GetRequiredService<TRepositoryManager>());
            services.AddSingleton(sp => sp.GetRequiredService<TRepositoryManager>().GetProvider(null));
            services.AddSingleton(sp =>
            {
                var m = new TRepositoryManager();
                void onChange(IConfiguration c)
                {
                    var section = c.GetSection(sectionName);
                    if(!section.Exists())
                    {
                        throw new System.InvalidProgramException("Cannot found 'MongoDB' section on 'appsettings.json' file or other configuration source.");
                    }
                    var connections = section.GetChildren().ToDictionary(item => item.Key, item => item.Value);
                    m.Initialize(connections);
                }
                var configuration = sp.GetRequiredService<IConfiguration>();
                Primitives.ChangeToken.OnChange(configuration.GetReloadToken, onChange, configuration);
                onChange(configuration);
                return m;
            });

            return services;
        }

        /// <summary>
        /// 添加 MongoDB 管理器
        /// </summary>
        /// <typeparam name="TRepositoryManager">仓库管理器的数据类型。</typeparam>
        /// <param name="services">服务集合。</param>
        /// <param name="configuration">配置。</param>
        /// <returns>服务集合。</returns>
        public static IServiceCollection AddRepositoryMongoDB<TRepositoryManager>(this IServiceCollection services, IConfiguration configuration)
           where TRepositoryManager : class, IRepositoryManager, new()
        {
            return services.AddRepositoryMongoDB<TRepositoryManager>(configuration.GetChildren().ToDictionary(item => item.Key, item => item.Value));
        }

        /// <summary>
        /// 添加 MongoDB 管理器
        /// </summary>
        /// <typeparam name="TRepositoryManager">仓库管理器的数据类型。</typeparam>
        /// <param name="services">服务集合。</param>
        /// <param name="connections">连接列表</param>
        /// <returns>服务集合。</returns>
        public static IServiceCollection AddRepositoryMongoDB<TRepositoryManager>(this IServiceCollection services, IReadOnlyDictionary<string, string> connections)
            where TRepositoryManager : class, IRepositoryManager, new()
        {
            var m = new TRepositoryManager();
            m.Initialize(connections);
            services.AddSingleton<IRepositoryManager>(m);
            services.AddSingleton(s => s.GetRequiredService<IRepositoryManager>().GetProvider(null));
            services.AddSingleton(m);
            return services;
        }
    }
}
