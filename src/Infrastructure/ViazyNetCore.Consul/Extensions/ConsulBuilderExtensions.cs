using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ViazyNetCore;
using ViazyNetCore.Consul;
using ViazyNetCore.Consul.Configurations;
using ViazyNetCore.Consul.Implementation;
using ViazyNetCore.Consul.Internal;
using ViazyNetCore.Consul.Selectors;
using ViazyNetCore.Gateway.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConsulBuilderExtensions
{
    public static IServiceCollection AddConsulServiceSubsribeManager(this IServiceCollection services, Action<ConfigInfo> options)
    {
        services.AddOptions();
        services.Configure(options);

        services.AddSingleton<ISerializer<byte[]>, StringByteArraySerializer>();
        services.AddSingleton<ISerializer<string>, JsonSerializer>();
        services.AddSingleton<IHealthCheckService, ConsulHealthCheckService>();
        services.AddSingleton<IClientWatchManager, ClientWatchManager>();
        services.AddSingleton<IConsulAddressSelector, ConsulRandomAddressSelector>();
        services.AddSingleton<IServiceSubscriberFactory, DefaultServiceSubscriberFactory>();
        services.AddSingleton<IConsulClientProvider, DefaultConsulClientProvider>();

        services.AddSingleton<IServiceSubscribeManager>(provider =>
        {
            var option = provider.GetService<IOptions<ConfigInfo>>()!.Value;
            var result = new ConsulServiceSubscribeManager(
                   option,
                   provider.GetRequiredService<ISerializer<byte[]>>(),
                   provider.GetRequiredService<ISerializer<string>>(),
                   provider.GetRequiredService<IClientWatchManager>(),
                   provider.GetRequiredService<IServiceSubscriberFactory>(),
                   provider.GetRequiredService<ILogger<ConsulServiceSubscribeManager>>(),
              provider.GetRequiredService<IConsulClientProvider>());
            return result;
        });

        return services;
    }

    public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
    {

        //获取consul配置实例
        var consulConfig = app.ApplicationServices.GetRequiredService<IOptions<ConfigInfo>>().Value;
        //获取应用程序声明周期事件
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        //var consulClient = new ConsulClient(c =>
        //{
        //    //consul服务注册地址
        //    c.Address = new Uri(consulConfig.ConsulAddress);
        //});

        ////服务注册配置
        //var registration = new AgentServiceRegistration()
        //{
        //    ID = Guid.NewGuid().ToString(),
        //    Name = consulConfig.Addresses,//服务名称
        //    Address = consulConfig.Host,//服务IP
        //    Port = consulConfig.Port,//服务端口
        //    Check = new AgentServiceCheck()
        //    {
        //        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动后多久注册服务
        //        Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
        //        HTTP = $"http://{consulConfig.Host}:{consulConfig.Port}{consulConfig.HealthCheck}",//健康检查地址
        //        Timeout = TimeSpan.FromSeconds(5)//超时时间
        //    }
        //};

        //服务注册
        //consulClient.Agent.ServiceRegister(registration).Wait();
        var serviceSubscribeManager = app.ApplicationServices.GetRequiredService<IServiceSubscribeManager>();
        ////应用程序结束时  取消注册
        lifetime.ApplicationStopping.Register(() =>
        {
            serviceSubscribeManager.ClearAsync().Wait();
        });

        return app;
    }
}
