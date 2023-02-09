using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;
using ViazyNetCore;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Caching.DependencyInjection;
using ViazyNetCore.Formatter.Response;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();


// Add services to the container.
var ServiceAssemblies = new Assembly?[]
{
    RuntimeHelper.GetAssembly("Caesar.Modules")
};

builder.Services.AddCustomApiVersioning();
builder.Services.AddJwtAuthentication(option => option = builder.Configuration.GetSection("Jwt").Get<JwtOption>());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger("ViazyNetCore-Manage");

builder.Services.AddFreeMySqlDb(builder.Configuration);
builder.Services.AddEventBus();
builder.Services.AddLocalCacheService();

builder.Services.AddApiDescriptor(option => {
    option.CachePrefix = "Viazy";
    option.ServiceName = "BMS";
});

//- 添加自动依赖注入
builder.Services.AddAssemblyServices(ServiceLifetime.Scoped, ServiceAssemblies);
builder.Services.RegisterEventHanldersDependencies(ServiceAssemblies, ServiceLifetime.Scoped);

var app = builder.Build();
app.UseFreeSql();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAndUI();
}
app.UseApiResponseWrapper(option =>
{
    option.EnableResponseLogging = true;
    option.EnableExceptionLogging = true;
});
app.MapControllers();

app.Run();
