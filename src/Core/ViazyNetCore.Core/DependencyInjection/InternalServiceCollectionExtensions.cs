using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore.Modules;

namespace ViazyNetCore.DependencyInjection
{
    internal static class InternalServiceCollectionExtensions
    {
        internal static void AddCoreServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();
            services.AddLocalization();
        }


        internal static void AddApplicationCoreServices(this IServiceCollection services,
           IApplication application,
           ApplicationCreationOptions applicationCreationOptions)
        {
            var moduleLoader = new ModuleLoader();

            services.TryAddSingleton<IModuleLoader>(moduleLoader);

            services.AddAssemblyOf<IApplication>();

            //services.AddTransient(typeof(ISimpleStateCheckerManager<>), typeof(SimpleStateCheckerManager<>));

            services.TryAddTransient<OnPreApplicationInitializationModuleLifecycleContributor>();
            services.TryAddTransient<OnApplicationInitializationModuleLifecycleContributor>();
            services.TryAddTransient<OnPostApplicationInitializationModuleLifecycleContributor>();
            services.TryAddTransient<OnApplicationShutdownModuleLifecycleContributor>();

            services.Configure<ModuleLifecycleOptions>(options =>
            {
                options.Contributors.AddIfNotContains(typeof(OnPreApplicationInitializationModuleLifecycleContributor));
                options.Contributors.AddIfNotContains(typeof(OnApplicationInitializationModuleLifecycleContributor));
                options.Contributors.AddIfNotContains(typeof(OnPostApplicationInitializationModuleLifecycleContributor));
                options.Contributors.AddIfNotContains(typeof(OnApplicationShutdownModuleLifecycleContributor));
            });
        }
    }

}
