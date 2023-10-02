using ViazyNetCore.Identity.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFreeDb(builder.Configuration.GetSection("dbConfig"));
// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddCaching();

await builder.AddApplicationAsync<AspNetCoreIdentityModules>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.InitializeApplication();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
