using System.Providers;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using NLog.Web;
using ViazyNetCore;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Caching.DependencyInjection;
using ViazyNetCore.Configuration;
using ViazyNetCore.DI;
using ViazyNetCore.ShopMall.AppApi.Extensions;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureLogging(logging =>
{
    logging.ClearProviders(); //�Ƴ��Ѿ�ע���������־�������
    logging.SetMinimumLevel(LogLevel.Trace); //������С����־����
})
.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = false;
})
.UseNLog();


// Add services to the container.
var ServiceAssemblies = new Assembly?[]
{
    RuntimeHelper.GetAssembly("ViazyNetCore.Modules")
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

builder.Services.AddSingleton(new AppSettingsHelper());

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new DynamicControllerGroupConvention());
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.InitializeDefault();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();


builder.Services.AddFreeMySqlDb();
builder.Services.AddEventBus();
// Redis �ֲ�ʽ����ע��
//builder.Services.AddRedisDistributedHashCache(options =>
//{
//    options.Configuration = AppSettingsConstVars.RedisConfigConnectionString;
//});
builder.Services.AddRumtimeCacheService();
builder.Services.AddSenparc(builder.Configuration);

//- ����Զ�����ע��
builder.Services.AddSingleton(sp => LockProvider.Default);
builder.Services.AddAssemblyServices(ServiceLifetime.Scoped, ServiceAssemblies);
builder.Services.RegisterEventHanldersDependencies(ServiceAssemblies, ServiceLifetime.Scoped);
builder.Services.AddShopMall();
builder.Services.AddSwagger(option =>
{
    option.Projects.Add(new ViazyNetCore.Swagger.ProjectConfig
    {
        Code = "shopmall",
        Description = "��̨����",
        Name = "ViazyNetCore",
        Version = "v2.0",
    });
});

var app = builder.Build();
app.UseFreeSql();
app.UseHttpsRedirection();
//app.UseDynamicController();
app.UseStaticFiles();

var filePath = app.Services.GetService<IWebHostEnvironment>()!.ContentRootFileProvider.GetDirectoryInfo("../files", true);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = filePath,
    RequestPath = "/upload"
});
app.UseRouting();
app.UseEventBusWithStore(ServiceAssemblies);
// Configure the HTTP response wrapper.
app.UseApiResponseWrapper(option =>
{
    option.IsApiOnly = false;
    option.EnableResponseLogging = true;
    option.EnableExceptionLogging = true;

    option.IsDebug = app.Environment.IsDevelopment();
});
app.UseSenparc(app.Environment, builder.Configuration);
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAndUI();
}
app.Run();
