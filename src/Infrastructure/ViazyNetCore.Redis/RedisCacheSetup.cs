using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ViazyNetCore.Configuration;

namespace ViazyNetCore.Redis
{
    public static class RedisCacheSetup
    {
        public static void AddRedisCacheSetup(this IServiceCollection services)
        {
            if(services == null) throw new ArgumentNullException(nameof(services));

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
