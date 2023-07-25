using System.Providers;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using ViazyNetCore;
using ViazyNetCore.AttachmentProvider;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Configuration;
using ViazyNetCore.Manage.WebApi;
using ViazyNetCore.MultiTenancy.AspNetCore;
using ViazyNetCore.TunnelWorks.ManageHost;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureLogging(logging =>
{
    logging.ClearProviders(); //�Ƴ��Ѿ�ע���������־��������
    logging.SetMinimumLevel(LogLevel.Trace); //������С����־����
})
.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = false;
})
.UseNLog();

builder.Configuration.ConfigBuild(builder.Environment);
builder.Services.AddSingleton(new AppSettingsHelper());
builder.Services.ReplaceConfiguration(builder.Configuration);

builder.Services.AddMultiTenancy();

//builder.Services.AddJwtAuthentication(builder.Configuration);
// Add application to the container.
await builder.Services.AddApplicationAsync<TunnelWorksManageHostModule>();

//builder.Services.AddCustomApiVersioning();

builder.Services.AddFreeDb(builder.Configuration.GetSection("dbConfig"));

builder.Services.AddCaching()
    .UseDistributedMemoryCache()
    //.UseStackExchangeRedisCaching(options =>
    //{
    //    var redisConfig = builder.Configuration.GetSection("Redis").Get<RedisConfig>();

    //    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
    //    {
    //        EndPoints =
    //        {
    //            { redisConfig.Host, redisConfig.Port }
    //        },
    //        Password = redisConfig.Password,
    //    };
    //})
    ;

//- �����Զ�����ע��

builder.Services.AddLocalStoreProvider(options =>
{
    //options.RequestPath
    options.StoreRootPath = "../files";
    options.RequestPath = "/upload";
    options.MediaTypes = new List<MediaType> { MediaType.Image };
});
builder.Services.AddSingleton(sp => LockProvider.Default);

builder.Services.AddHealthChecks();
builder.Services.AddResponseCompression();

var app = builder.Build();
app.InitializeApplication();
app.UseMultiTenancyFreeSql();
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

if(app.Environment.IsDevelopment())
{
    app.UseSwaggerAndUI();
    app.UseSpa(spa =>
    {
        // ǰ���ļ�Ŀ¼
        spa.Options.SourcePath = "../../../fontend/tunnelworks-front";
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