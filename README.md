<h1 align="center"> Viazy NetCore 业务项目</h1>

> 这目前只是一个简单的 .Net6 后台管理项目。

## 🚩 项目介绍 
- ViazyNetCore.Data.FreeSql 引入 FreeSql 作为数据ORM框架
- ViazyNetCore.Auth 权限管理模块
- ViazyNetCore.Caching 缓存模块
- ViazyNetCore.Redis Redis及缓存管理
- ViazyNetCore.Swagger Swagger管理及Knife4jUI
- ViazyNetCore.EventBus 事件推送模块
- ViazyNetCore.Formmatter.Response 公共处理返回业务模块。
- ViazyNetCore.Formmatter.Excel 数据转Excel文件下载模块
- ViazyNetCore.Web.DevServer ViteNode Spa处理模块。
- ViazyNetCore.TaskScheduler 基于Quartz.Net的任务管理。
- ViazyNetCore.RabbitMQ RabbitMQ消息队列
- ViazyNetCore.TaskScheduler.RabbitMQ RabbitMQ Quartz任务消费者
- ViazyNetCore.OSS  OSS 文件存储基类
- ViazyNetCore.OSS.Minio MinIO 文件存储
-
- fontend/ele-admin-ui vue3 管理后台-前端UI
- fontend/shopmall-uniapp 前端商城小程序 uni-app项目
## 🚀 快速入门

> 前端管理后台使用 Vite + Vue3 + TypeScripe +ElementUI + PureAdmin
> 前端商城使用 uni-app nvue3 + TypeScripe + kv-uview-ui
> SwaggerUI 项目采用 knife4j-vue 并调整 TypeScripe文档生成

> 示范

``` csharp

// 模块注入
builder.Services.AddCaching()  // 缓存注入
    .UseDistributedMemoryCache()  // 内存缓存
    .UseStackExchangeRedisCaching(options =>  // 基于 StackExchangeRedis 的缓存
    {
        var redisConfig = builder.Configuration.GetSection("Redis").Get<RedisConfig>();

        options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
        {
            EndPoints =
            {
                { redisConfig.Host, redisConfig.Port }
            },
            Password = redisConfig.Password,
            ChannelPrefix = "Blog"
        };
    });
await builder.Services.AddApplicationAsync<BloggingManageHostModule>();

// Api 返回全局拦截及处理
app.UseApiResponseWrapper(option =>
{
    option.IsApiOnly = false;
    option.EnableResponseLogging = true;
    option.EnableExceptionLogging = true;
});

// Environment.IsDevelopment()
 app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "client"; //启用的前端项目的路径 相对于当前项目路径
        //spa.Options.PackageManagerCommand = "npm"; // 执行的 command命令
        // 开发阶段, 启用 ViteNode 监听端口，前后端可单端口运行，F5 一键启动调试。
        spa.UseDevServer(new ViteNodeServerOptions()  // dotnet add package ViazyNetCore.Web.DevServer
        {
            //Host= "172.0.0.1",
        });
    });
```

```csharp
[DependsOn(typeof(AutoMapperModule)
        , typeof(IdentityModule)
        , typeof(AspNetCoreMvcModule)
        , typeof(AuthorizationModule)
        , typeof(AuthApplicationModule)
        , typeof(BloggingModulsModule)
        )]
    public class BloggingManageHostModule : InjectionModule
    {
        …
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // dotnet add package ViazyNetCore.Swagger
            Configure<SwaggerConfig>(options =>
            {
                options.Projects.Add(new ProjectConfig
                {
                    Name = "博客",
                    Code = "blogging",
                    Description = "博客",
                    Version = "1.0",
                });
            });
            context.Services.AddSwagger();
        }
    }
```