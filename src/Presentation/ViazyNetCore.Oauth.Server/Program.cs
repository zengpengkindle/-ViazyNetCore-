using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
builder.Services.AddControllers();

builder.Services.ConfigureOpenIddictServices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:7119";
        options.Audience = "APIResource";
    });

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
app.UseAuthorization(); 
app.UseViazyOpenIddictValidation(JwtBearerDefaults.AuthenticationScheme);

//app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerAndUI();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
