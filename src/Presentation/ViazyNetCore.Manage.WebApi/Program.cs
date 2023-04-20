using System.Providers;
using System.Reflection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog.Web;
using ViazyNetCore;
using ViazyNetCore.AttachmentProvider;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Caching.DependencyInjection;
using ViazyNetCore.Configuration;
using ViazyNetCore.DI;
using ViazyNetCore.Modules.Internal;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureLogging(logging =>
{
    logging.ClearProviders(); //移除已经注册的其他日志处理程序
    logging.SetMinimumLevel(LogLevel.Trace); //设置最小的日志级别
})
.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = false;
})
.UseNLog();

builder.Configuration.ConfigBuild(builder.Environment);

// Add services to the container.
var ServiceAssemblies = new Assembly?[]
{
    RuntimeHelper.GetAssembly("ViazyNetCore.ShopMall.Modules"),
    RuntimeHelper.GetAssembly("ViazyNetCore.Authorization")
};
var autoMapperIoc = new Assembly?[]
{
    RuntimeHelper.GetAssembly("ViazyNetCore.ShopMall.Modules"),
    RuntimeHelper.GetAssembly("ViazyNetCore.Manage.WebApi")
};
builder.Services.AddJwtAuthentication(option =>
{
    var optionJson = builder.Configuration.GetSection("Jwt").Get<JwtOption>();
    option.Secret = optionJson.Secret;
    option.ExpiresIn = optionJson.ExpiresIn;
    option.Issuer = optionJson.Issuer;
    option.AppName = optionJson.AppName;
    option.UseDistributedCache = true;
});
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AllowNullCollections = true;
}, autoMapperIoc);
builder.Services.AddSingleton(new AppSettingsHelper());

builder.Services.AddControllers(options =>
{
    options.Filters.Add<PermissionFilter>();
    options.Conventions.Add(new DynamicControllerGroupConvention());
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.InitializeDefault();
})
.AddApplicationPart(typeof(UserOption).Assembly)
.AddApplicationPart(typeof(JobSetup).Assembly);

builder.Services.AddAuthorizationController();

//builder.Services.AddCustomApiVersioning();
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();


builder.Services.AddFreeDb(builder.Configuration.GetSection("dbConfig"));
builder.Services.AddEventBus();
// Redis 分布式缓存注入
//builder.Services.AddRedisDistributedHashCache(options =>
//{
//    options.Configuration = AppSettingsConstVars.RedisConfigConnectionString;
//});
builder.Services.AddRumtimeCacheService();

builder.Services.AddApiDescriptor(option =>
{
    option.CachePrefix = "Viazy";
    option.ServiceName = "BMS";
});
//- 添加自动依赖注入
builder.Services.AddAssemblyServices(ServiceLifetime.Scoped, ServiceAssemblies);
builder.Services.RegisterEventHanldersDependencies(ServiceAssemblies, ServiceLifetime.Scoped);

builder.Services.AddMQueue();
builder.Services.AddJobSetup();
builder.Services.AddJobTaskSetup();

builder.Services.AddLocalStoreProvider(options =>
{
    //options.RequestPath
    options.StoreRootPath = "../files";
    options.RequestPath = "/upload";
    options.MediaTypes = new List<MediaType> { MediaType.Image };
});
builder.Services.AddSingleton(sp => LockProvider.Default);
builder.Services.AddShopMall();
builder.Services.AddSwagger(option =>
{
    option.Projects.Add(new ViazyNetCore.Swagger.ProjectConfig
    {
        Code = "admin",
        Description = "后台管理",
        Name = "ViazyNetCore",
        Version = "v2.0",
    });
    option.Projects.Add(new ViazyNetCore.Swagger.ProjectConfig
    {
        Code = "shopmall",
        Description = "ShopMall",
        Name = "ShopMall",
        Version = "v1",
    });

    option.Projects.Add(new ViazyNetCore.Swagger.ProjectConfig
    {
        Code = "task",
        Description = "TaskJob",
        Name = "TaskJob",
        Version = "v1",
    });
});

builder.Services.AddHealthChecks();
builder.Services.AddResponseCompression();


var app = builder.Build();
app.UseFreeSql();
app.UseHttpsRedirection();
//app.UseDynamicController();
app.UseStaticFiles();
app.UseStoreProvider();
app.UseRouting();
app.UseEventBusWithStore(ServiceAssemblies);
// Configure the HTTP response wrapper.
app.UseApiResponseWrapper(option =>
{
    option.IsApiOnly = false;
    option.EnableResponseLogging = true;
    option.EnableExceptionLogging = true;
    option.BypassHTMLValidation = true;

    option.IsDebug = app.Environment.IsDevelopment();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAndUI();
    app.UseSpa(spa =>
    {
        // 前端文件目录
        spa.Options.SourcePath = "../../../fontend/ele-admin-ui";
        //spa.Options.PackageManagerCommand = "pnpm";
        spa.UseDevServer(new System.Web.DevServer.ViteNodeServerOptions()
        {
            //Host= "172.0.0.1",
        });
    });
}
else
{
    app.UseHsts()
        //.UseThreadsPreheat()
        .UseHistoryFallback();
}
app.Run();
