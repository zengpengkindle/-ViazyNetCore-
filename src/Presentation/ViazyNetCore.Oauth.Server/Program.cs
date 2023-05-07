using System.Reflection;
using Matty.Server;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
using ViazyNetCore;
using ViazyNetCore.Caching.DependencyInjection;
using ViazyNetCore.Oauth.Server;

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
builder.Services.AddRumtimeCacheService();

builder.Services.AddHostedService<Worker>();

await builder.Services.AddApplicationAsync<OauthServerApplicationModule>();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMultiTenancy();

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
app.InitializeApplication();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseOpenIddictValidation();
app.UseAuthorization(); 

//app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerAndUI();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
