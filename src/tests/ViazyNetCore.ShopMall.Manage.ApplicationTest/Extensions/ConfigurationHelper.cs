using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ConfigurationHelper
    {
        public static void ConfigBuild(this IConfigurationBuilder configurationManager, string? env)
        {
            configurationManager.AddJsonFile("./Configs/dbconfig.json", optional: true, reloadOnChange: true);
            if (env.IsNotNull())
            {
                configurationManager.AddJsonFile($"./Configs/dbconfig.{env}.json", optional: true, reloadOnChange: true);
            }

            configurationManager.AddJsonFile("./Configs/kuaishou.json", optional: true, reloadOnChange: true);
            if (env.IsNotNull())
            {
                configurationManager.AddJsonFile($"./Configs/kuaishou.{env}.json", optional: true, reloadOnChange: true);
            }
            configurationManager.AddJsonFile("./Configs/douyin.json", optional: true, reloadOnChange: true);
            if (env.IsNotNull())
            {
                configurationManager.AddJsonFile($"./Configs/douyin.{env}.json", optional: true, reloadOnChange: true);
            }

            configurationManager.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (env.IsNotNull())
            {
                configurationManager.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            }
        }
    }
}
