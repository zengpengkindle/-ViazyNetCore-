using System.Reflection;
using NLog.Web;
using ViazyNetCore;
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger("ViazyNetCore-Manage");

builder.Services.AddFreeMySqlDb(builder.Configuration);
builder.Services.AddEventBus();

//- 添加自动依赖注入
builder.Services.AddAssemblyServices(ServiceLifetime.Scoped, ServiceAssemblies);
builder.Services.RegisterEventHanldersDependencies(ServiceAssemblies, ServiceLifetime.Scoped);
//注入Options
builder.Services.Configure<ResponseWrapperOptions>(builder.Configuration.GetSection("ResponseWrapper"));

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
app.MapControllers();

app.Run();
