using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.AspNetCore.Core;
using ViazyNetCore.Data.FreeSql;
using ViazyNetCore.Identity;
using ViazyNetCore.IdentityService4.FreeSql;
using ViazyNetCore.IdentityService4.SampleWeb.DataSeeder;
using ViazyNetCore.Modules;

namespace ViazyNetCore.IdentityService4.SampleWeb
{
    [DependsOn(typeof(AspNetCoreMvcModule)
        , typeof(IdentityAuthModule))]
    public class ViazyNetCoreIdentityService4SampleWebModule : InjectionModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AppOptions>(options =>
            {
                options.AppType = AppType.ControllersWithViews;
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = services.GetConfiguration();

            services.AddFreeDb(configuration.GetSection("dbconfig"));
            Configure<DbConfig>(options =>
            {
                options.Buider = builder =>
                {
                    var configurationDbContext = builder.Build<ConfigurationDbContext>();
                    var persistedGrantDbContext = builder.Build<PersistedGrantDbContext>();
                };
            });

            //services.GetSingletonInstance<IFreeSql>();

            Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.IsEssential = true;
                // we need to disable to allow iframe for authorize requests
                options.Cookie.SameSite = SameSiteMode.None;
                options.LogoutPath= "/Account/LoggedOut";
            });
            services.ConfigureExternalCookie(options =>
            {
                options.Cookie.IsEssential = true;
                // https://github.com/IdentityServer/IdentityServer4/issues/2595
                options.Cookie.SameSite = SameSiteMode.None;
            });

            var serivceProvider = services.BuildServiceProvider();
            var freeSql = serivceProvider.GetService<IFreeSql>();

            services.AddAuthentication()
                .AddCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.LogoutPath= "/Account/LoggedOut";
                    options.SlidingExpiration = true;
                });
            Configure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "idsrv";
                options.DefaultChallengeScheme = "idsrv";
                options.DefaultSignOutScheme = "idsrv";
            });

            var builder = services.AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseFreeSql(orm: freeSql);
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseFreeSql(orm: freeSql);

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600;
            })
            .AddProfileService<DefaultProfileService>();
#if DEBUG
            builder.AddDeveloperSigningCredential();
#else

            string certPath = Path.Combine(AppContext.BaseDirectory, configuration.GetValue<string>("Cert:Path"));
                string certPwd = configuration.GetValue<string>("Cert:Password");
                if(!File.Exists(certPath))
                {
                    throw new Exception("IdentityServer证书路径错误。");
                }
                builder.AddConfigurationStoreCache();
                builder.AddSigningCredential(new X509Certificate2(certPath, certPwd));
#endif
            services.AddTransient<ClientDataSeeder>();
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseIdentityServer();

            using var scope = app.ApplicationServices.CreateScope();
            var clientDataSeeder = scope.ServiceProvider.GetRequiredService<ClientDataSeeder>();
            clientDataSeeder.CreateClientAsync().Wait();
        }
    }
}
