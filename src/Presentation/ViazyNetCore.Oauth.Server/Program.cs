using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.ConfigBuild(builder.Environment);

builder.Services.AddFreeDb(builder.Configuration.GetSection("dbConfig"));
builder.Services.AddOpenIddictServer();

var app = builder.Build();
app.UseFreeSql();

app.UseAuthentication();
app.UseAuthorization();
app.UseViazyOpenIddictValidation();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
