using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Modules;

namespace ViazyNetCore
{
    public class ApplicationCreationOptions
    {
        [NotNull]
        public IServiceCollection Services { get; }

        public PlugInSourceList PlugInSources { get; }

        /// <summary>
        /// The options in this property only take effect when IConfiguration not registered.
        /// </summary>
        [NotNull]
        public ConfigurationBuilderOptions Configuration { get; }

        public bool SkipConfigureServices { get; set; }

        public string ApplicationName { get; set; }

        public string Environment { get; set; }

        public ApplicationCreationOptions([NotNull] IServiceCollection services)
        {
            Services = Check.NotNull(services, nameof(services));
            PlugInSources = new PlugInSourceList();
            Configuration = new ConfigurationBuilderOptions();
        }
    }
}
