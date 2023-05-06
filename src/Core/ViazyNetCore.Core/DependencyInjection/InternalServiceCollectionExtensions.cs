﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
           IApplication abpApplication,
           ApplicationCreationOptions applicationCreationOptions)
        {
            var moduleLoader = new ModuleLoader();
            //var assemblyFinder = new AssemblyFinder(abpApplication);
            //var typeFinder = new TypeFinder(assemblyFinder);

            //if (!services.IsAdded<IConfiguration>())
            //{
            //    services.ReplaceConfiguration(
            //        ConfigurationHelper.BuildConfiguration(
            //            applicationCreationOptions.Configuration
            //        )
            //    );
            //}

            services.TryAddSingleton<IModuleLoader>(moduleLoader);
            //services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
            //services.TryAddSingleton<ITypeFinder>(typeFinder);
            //services.TryAddSingleton<IInitLoggerFactory>(new DefaultInitLoggerFactory());

            services.AddAssemblyOf<IApplication>();

            //services.AddTransient(typeof(ISimpleStateCheckerManager<>), typeof(SimpleStateCheckerManager<>));

            services.TryAddTransient<OnApplicationInitializationModuleLifecycleContributor>();

            services.Configure<ModuleLifecycleOptions>(options =>
            {
                //options.Contributors.Add<OnPreApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.AddIfNotContains(typeof(OnApplicationInitializationModuleLifecycleContributor));
                //options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
                //options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
            });
        }
    }

}