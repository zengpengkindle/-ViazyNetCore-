using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.Identity;
using ViazyNetCore.IdentityService4.FreeSql;

namespace ViazyNetCore.IdentityService4.SampleWeb
{
    [DependsOn(typeof(AspNetCoreMvcModule)
        , typeof(IdentityAuthModule))]
    public class ViazyNetCoreIdentityService4SampleWebModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = services.GetConfiguration();

            services.AddFreeDb(configuration.GetSection("dbconfig"));

            var freeSql = services.GetSingletonInstance<IFreeSql>();
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
                });
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
        }
    }
}
