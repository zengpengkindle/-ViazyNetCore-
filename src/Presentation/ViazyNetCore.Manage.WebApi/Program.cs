using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;
using ViazyNetCore;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Caching.DependencyInjection;
using ViazyNetCore.Formatter.Response;

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

builder.Services.AddCustomApiVersioning();
builder.Services.AddJwtAuthentication(option => option = builder.Configuration.GetSection("Jwt").Get<JwtOption>());
builder.Services.AddControllers();
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

//- ����Զ�����ע��
builder.Services.AddAssemblyServices(ServiceLifetime.Scoped, ServiceAssemblies);
builder.Services.RegisterEventHanldersDependencies(ServiceAssemblies, ServiceLifetime.Scoped);

var app = builder.Build();
app.UseFreeSql();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseStaticFiles();
// Configure the HTTP request pipeline.
app.MapControllers();
app.UseApiResponseWrapper(option =>
{
    //option.IsApiOnly = false;
    option.EnableResponseLogging = true;
    option.EnableExceptionLogging = true;
});

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAndUI();
}
if (app.Environment.IsDevelopment())
{
    //app.UseSpa(spa =>
    //{
    //    spa.Options.SourcePath = "client";
    //    //spa.Options.PackageManagerCommand = "pnpm";
    //    spa.UseDevServer(new System.Web.DevServer.ViteNodeServerOptions()
    //    {
    //        //Host= "172.23.48.1",
    //    });
    //});
}
else
{
    app.UseHsts()
        //.UseThreadsPreheat()
        .UseHistoryFallback();
}
app.Run();
