<h1 align="center"> Viazy NetCore 业务项目</h1>

> 这目前只是一个简单的 .Net6 后台管理项目。

## 🚩 组件介绍 
- ViazyNetCore.Data.FreeSql 引入 FreeSql 作为数据ORM框架
- ViazyNetCore.Auth 权限管理模块
- ViazyNetCore.Caching 缓存模块
- ViazyNetCore.Redis Redis及缓存管理
- ViazyNetCore.Swagger Swagger管理及Knife4jUI
- ViazyNetCore.EventBus 事件推送模块
- ViazyNetCore.Formmatter.Response 公共处理返回业务模块。
- ViazyNetCore.Formmatter.Excel 数据转Excel文件下载模块
- ViazyNetCore.Web.DevServer ViteNode Spa处理模块。
- 
## 🚀 快速入门

> 前端使用 Vite + Vue3 + TypeScripe +ElementUI + PureAdmin 

> SwaggerUI 项目采用 knife4j-vue 并调整 TypeScripe文档生成

> 示范

``` csharp
builder.Services.AddCustomApiVersioning(); // 启用Api版本管理
builder.Services.AddJwtAuthentication() // 启用Jwt授权

builder.Services.AddSwagger("ViazyNetCore-Manage");//注入Swagger文档
builder.Services.AddEventBus();// 注入EventBus 事件推送器
// 注入EventBus 事件Handler
builder.Services.RegisterEventHanldersDependencies(ServiceAssemblies, ServiceLifetime.Scoped);

builder.Services.AddApiDescriptor(); //Api接口文档获取器
//- 添加自动依赖注入
builder.Services.AddAssemblyServices(ServiceLifetime.Scoped, ServiceAssemblies);

// Api 返回全局拦截及处理
app.UseApiResponseWrapper(option =>
{
    option.IsApiOnly = false;
    option.EnableResponseLogging = true;
    option.EnableExceptionLogging = true;
});

// Environment.IsDevelopment()
 app.UseSwaggerAndUI(); // 启用SwaggerUI
 app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "client"; //启用的前端项目的路径 相对于当前项目路径
        //spa.Options.PackageManagerCommand = "npm"; // 执行的 command命令
        // 开发阶段, 启用 ViteNode 监听端口，前后端可单端口运行，F5 一键启动调试。
        spa.UseDevServer(new System.Web.DevServer.ViteNodeServerOptions() 
        {
            //Host= "172.0.0.1",
        });
    });
```