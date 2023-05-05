using System.Reflection;
using Matty.Server;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore;
using ViazyNetCore.Caching.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureLogging(logging =>
{
    logging.ClearProviders(); //�Ƴ��Ѿ�ע���������־�������
    logging.SetMinimumLevel(LogLevel.Trace); //������С����־����
})
.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = false;
});
// Add services to the container.
builder.Configuration.ConfigBuild(builder.Environment);
builder.Services.AddFreeDb(builder.Configuration.GetSection("dbConfig"));
builder.Services.AddRumtimeCacheService();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenIddictIdentity(options => {
    options.Password = new Microsoft.AspNetCore.Identity.PasswordOptions
    {
         RequireDigit=false,
    };
});
builder.Services.ConfigureOpenIddictServices();


builder.Services.AddControllers();
var ServiceAssemblies = new Assembly?[]
{
    RuntimeHelper.GetAssembly("ViazyNetCore.Authorization")
};
builder.Services.AddAssemblyServices(ServiceLifetime.Scoped, ServiceAssemblies);
var autoMapperIoc = new Assembly?[]
{
    RuntimeHelper.GetAssembly("ViazyNetCore.OpenIddict"),
    RuntimeHelper.GetAssembly("ViazyNetCore.Authorization")
};
builder.Services.AddHttpContextAccessor();
builder.Services.AddMultiTenancy();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AllowNullCollections = true;
}, autoMapperIoc);

builder.Services.AddSwaggerGen();
builder.Services.AddSwagger(option =>
{
    option.Projects.Add(new ViazyNetCore.Swagger.ProjectConfig
    {
        Code = "openiddict",
        Description = "OAuth OpenIddict",
        Name = "ViazyNetCore OpenIddict",
        Version = "v1.0",
    });
});
var app = builder.Build();
app.UseFreeSql();

app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseOpenIddictValidation();
app.UseAuthorization(); 

//app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerAndUI();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
