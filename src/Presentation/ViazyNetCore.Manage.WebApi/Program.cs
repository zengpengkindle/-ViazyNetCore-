using System.Reflection;
using Newtonsoft.Json;
using NLog.Web;
using ViazyNetCore;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Caching.DependencyInjection;
using ViazyNetCore.Manage.WebApi.Controllers;

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


// Add services to the container.
var ServiceAssemblies = new Assembly?[]
{
    RuntimeHelper.GetAssembly("ViazyNetCore.Modules"),
    RuntimeHelper.GetAssembly("ViazyNetCore.Auth")
};

builder.Services.AddCustomApiVersioning();
builder.Services.AddJwtAuthentication(option =>
{
    var optionJson = builder.Configuration.GetSection("Jwt").Get<JwtOption>();
    option.Secret = optionJson.Secret;
    option.ExpiresIn = optionJson.ExpiresIn;
    option.Issuer = optionJson.Issuer;
    option.AppName = optionJson.AppName;
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add<PermissionFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.InitializeDefault();
});
//.AddApplicationPart(typeof(TestController).Assembly)
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger("ViazyNetCore-Manage");

builder.Services.AddFreeMySqlDb(builder.Configuration);
builder.Services.AddEventBus();
builder.Services.AddRumtimeCacheService();


builder.Services.AddApiDescriptor(option =>
{
    option.CachePrefix = "Viazy";
    option.ServiceName = "BMS";
});

//- 添加自动依赖注入
builder.Services.AddAssemblyServices(ServiceLifetime.Scoped, ServiceAssemblies);
builder.Services.RegisterEventHanldersDependencies(ServiceAssemblies, ServiceLifetime.Scoped);

var app = builder.Build();
app.UseFreeSql();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// Configure the HTTP response wrapper.
app.UseApiResponseWrapper(option =>
{
    option.IsApiOnly = false;
    option.EnableResponseLogging = true;
    option.EnableExceptionLogging = true;
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
        spa.Options.SourcePath = "client";
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
