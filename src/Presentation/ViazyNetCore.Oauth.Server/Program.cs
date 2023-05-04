using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenIddict.Validation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Matty.Server;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureLogging(logging =>
{
    logging.ClearProviders(); //移除已经注册的其他日志处理程序
    logging.SetMinimumLevel(LogLevel.Trace); //设置最小的日志级别
})
.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = false;
});
// Add services to the container.
builder.Configuration.ConfigBuild(builder.Environment);
builder.Services.AddFreeDb(builder.Configuration.GetSection("dbConfig"));

builder.Services.AddHostedService<Worker>();
var keyclockSection = builder.Configuration.GetSection("Keycloak");
builder.Services.ConfigureOpenIddictServices();


builder.Services.AddControllers();

var autoMapperIoc = new Assembly?[]
{
    RuntimeHelper.GetAssembly("ViazyNetCore.OpenIddict")
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
