using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ConfigurationHelper
    {
        public static void ConfigBuild(this ConfigurationManager configurationManager, IHostEnvironment env)
        {
            configurationManager.AddJsonFile("./Configs/ratelimitconfig.json", optional: true, reloadOnChange: true);
            if (env.EnvironmentName.IsNotNull())
            {
                configurationManager.AddJsonFile($"./Configs/ratelimitconfig.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            configurationManager.AddJsonFile("./Configs/dbconfig.json", optional: true, reloadOnChange: true);
            if (env.EnvironmentName.IsNotNull())
            {
                configurationManager.AddJsonFile($"./Configs/dbconfig.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            configurationManager.AddJsonFile("./Configs/rabbitmq.json", optional: true, reloadOnChange: true);
            if (env.EnvironmentName.IsNotNull())
            {
                configurationManager.AddJsonFile($"./Configs/rabbitmq.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }
            configurationManager.AddJsonFile("./Configs/redis.json", optional: true, reloadOnChange: true);
            if (env.EnvironmentName.IsNotNull())
            {
                configurationManager.AddJsonFile($"./Configs/redis.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            configurationManager.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (env.EnvironmentName.IsNotNull())
            {
                configurationManager.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }
        }
    }
}
