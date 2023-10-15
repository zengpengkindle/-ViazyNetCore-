using ViazyNetCore.IdentityService4.SampleWeb;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.ConfigBuild(builder.Environment);
builder.Services.AddCaching()
    .UseDistributedMemoryCache();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(120);
});

await builder.AddApplicationAsync<ViazyNetCoreIdentityService4SampleWebModule>();

var app = builder.Build();
app.InitializeApplication();
// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
