using System.Providers;
using System.Reflection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog.Web;
using ViazyNetCore;
using ViazyNetCore.AttachmentProvider;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.Caching.DependencyInjection;
using ViazyNetCore.Configuration;
using ViazyNetCore.DI;
using ViazyNetCore.Identity;
using ViazyNetCore.Manage.WebApi;
using ViazyNetCore.Modules;
using ViazyNetCore.Modules.Internal;

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

builder.Configuration.ConfigBuild(builder.Environment);


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

// Add application to the container.
await builder.Services.AddApplicationAsync<BmsApplicationModule>();

//builder.Services.AddCustomApiVersioning();

builder.Services.AddFreeDb(builder.Configuration.GetSection("dbConfig"));
// Redis �ֲ�ʽ����ע��
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

//- ����Զ�����ע��

builder.Services.AddLocalStoreProvider(options =>
{
    //options.RequestPath
    options.StoreRootPath = "../files";
    options.RequestPath = "/upload";
    options.MediaTypes = new List<MediaType> { MediaType.Image };
});
builder.Services.AddSingleton(sp => LockProvider.Default);
builder.Services.AddShopMall();

builder.Services.AddHealthChecks();
builder.Services.AddResponseCompression();


var app = builder.Build();
app.InitializeApplication();
app.UseFreeSql();
app.UseHttpsRedirection();
//app.UseDynamicController();
app.UseStaticFiles();
app.UseStoreProvider();
app.UseRouting();
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
        // ǰ���ļ�Ŀ¼
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
