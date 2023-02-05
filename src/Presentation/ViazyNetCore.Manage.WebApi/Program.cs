var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCustomApiVersioning();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger("ViazyNetCore-Manage");
var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwaggerAndUI("ViazyNetCore-Manage");
}

app.Run();
