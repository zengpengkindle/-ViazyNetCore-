using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ViazyNetCore.Dapr
{
    [DependsOn(typeof(SerializerModule))]
    public class DaprModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            ConfigureDaprOptions(configuration);

            context.Services.TryAddSingleton(
                serviceProvider => serviceProvider
                    .GetRequiredService<IDaprClientFactory>()
                    .Create()
            );
        }
        private void ConfigureDaprOptions(IConfiguration configuration)
        {
            Configure<DaprOptions>(configuration.GetSection("Dapr"));
            Configure<DaprOptions>(options =>
            {
                if (options.DaprApiToken.IsNullOrWhiteSpace())
                {
                    var confEnv = configuration["DAPR_API_TOKEN"];
                    if (!confEnv.IsNullOrWhiteSpace())
                    {
                        options.DaprApiToken = confEnv;
                    }
                    else
                    {
                        var env = Environment.GetEnvironmentVariable("DAPR_API_TOKEN");
                        if (!env.IsNullOrWhiteSpace())
                        {
                            options.DaprApiToken = env;
                        }
                    }
                }

                if (options.AppApiToken.IsNullOrWhiteSpace())
                {
                    var confEnv = configuration["APP_API_TOKEN"];
                    if (!confEnv.IsNullOrWhiteSpace())
                    {
                        options.AppApiToken = confEnv;
                    }
                    else
                    {
                        var env = Environment.GetEnvironmentVariable("APP_API_TOKEN");
                        if (!env.IsNullOrWhiteSpace())
                        {
                            options.AppApiToken = env;
                        }
                    }
                }
            });
        }
    }
}
