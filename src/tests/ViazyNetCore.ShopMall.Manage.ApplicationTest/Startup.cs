using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ViazyNetCore.Manage.WebApi;
using Xunit.DependencyInjection;

namespace ViazyNetCore.ShopMall.Manage.ApplicationTest
{
    public class Startup
    {
        public Startup()
        {
        }

        // 自定义 host 构建
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder
                .ConfigureAppConfiguration(builder =>
                {
                    builder.ConfigBuild("Development");
                })
                .ConfigureServices(async services =>
                {
                    await services.AddApplicationAsync<ShopMallBmsApplicationTestModule>();
                });

        }

        /// <summary>
        /// 可以添加要用到的方法参数，会自动从注册的服务中获取服务实例，类似于 asp.net core 里 Configure 方法
        /// </summary>
        /// <param name="provider"></param>
        public void Configure(IServiceProvider provider)
        {
        }
    }
}
