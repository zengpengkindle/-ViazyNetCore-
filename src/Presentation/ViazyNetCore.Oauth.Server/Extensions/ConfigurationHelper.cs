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
            configurationManager.AddJsonFile("./Configs/dbconfig.json", optional: true, reloadOnChange: true);
            if (env.EnvironmentName.IsNotNull())
            {
                configurationManager.AddJsonFile($"./Configs/dbconfig.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            configurationManager.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (env.EnvironmentName.IsNotNull())
            {
                configurationManager.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }
        }
    }
}
