using System.Providers;
using NLog.Web;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Configuration;
using ViazyNetCore.Modules.Internal;
using ViazyNetCore.ShopMall.AppApi;
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
builder.Configuration.ConfigBuild(builder.Environment);

// Add services to the container.

await builder.Services.AddApplicationAsync<ShopMallApiApplicationModule>();

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

builder.Services.AddFreeDb(builder.Configuration.GetSection("dbConfig"));
// Redis �ֲ�ʽ����ע��
//builder.Services.AddRedisDistributedHashCache(options =>
//{
//    options.Configuration = AppSettingsConstVars.RedisConfigConnectionString;
//});
builder.Services.AddRumtimeCacheService();
builder.Services.AddSenparc(builder.Configuration);

//- ����Զ�����ע��
builder.Services.AddSingleton(sp => LockProvider.Default);
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

builder.Services.AddHealthChecks();
builder.Services.AddResponseCompression();

var app = builder.Build();
app.UseFreeSql();
//app.UseHttpsRedirection();
//app.UseDynamicController();
app.UseStaticFiles();

var filePath = app.Services.GetService<IWebHostEnvironment>()!.ContentRootFileProvider.GetDirectoryInfo("../files", true);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = filePath,
    RequestPath = "/upload"
});
app.UseRouting();
// Configure the HTTP response wrapper.
app.UseApiResponseWrapper(option =>
{
    option.IsApiOnly = true;
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
