﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ViazyNetCore.DI;
using ViazyNetCore.Modules;
using ViazyNetCore.Mvc;

namespace ViazyNetCore.AspNetCore
{
    [DependsOn(typeof(AspNetCoreModule))]
    public class AspNetCoreMvcModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            context.Services.AddAuthorization();
            AddAspNetServices(context.Services);

            var appOptions = context.Services.ExecutePreConfiguredActions<AppOptions>();

            void mvcConfigure(MvcOptions options)
            {
                options.Conventions.Add(new DynamicControllerGroupConvention());
            }

            var mvcBuilder = appOptions.AppType switch
            {
                AppType.Controllers => services.AddControllers(mvcConfigure),
                AppType.ControllersWithViews => services.AddControllersWithViews(mvcConfigure),
                AppType.MVC => services.AddMvc(mvcConfigure),
                _ => services.AddControllers(mvcConfigure)
            };

            //Add feature providers
            var partManager = context.Services.GetSingletonInstance<ApplicationPartManager>();
            var application = context.Services.GetSingletonInstance<IApplication>();

            foreach (var featureProviders in appOptions.FeatureProviders)
            {
                partManager.FeatureProviders.AddIfNotContains(featureProviders);
            }

            AddToApplicationParts(partManager, appOptions.ApplicationParts);

            context.Services.AddOptions<MvcOptions>();
            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.InitializeDefault();
            });

        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            ApplicationPartSorter.Sort(
                context.Services.GetSingletonInstance<ApplicationPartManager>(),
                context.Services.GetSingletonInstance<IModuleContainer>()
            );
        }

        private static void AddAspNetServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            //AddApplicationParts(context);
        }

        private static void AddApplicationParts(ApplicationInitializationContext context)
        {
            var partManager = context.ServiceProvider.GetService<ApplicationPartManager>();
            if (partManager == null)
            {
                return;
            }

            //Plugin modules
            var moduleAssemblies = context
                .ServiceProvider
                .GetRequiredService<IModuleContainer>()
                .Modules
                .Where(m => m.IsLoadedAsPlugIn)
                .Select(m => m.Type.Assembly)
                .Distinct();

            AddToApplicationParts(partManager, moduleAssemblies);

            //Controllers for application services
            //var controllerAssemblies = context
            //    .ServiceProvider
            //    .GetRequiredService<IOptions<AspNetCoreMvcOptions>>()
            //    .Value
            //    .ConventionalControllers
            //    .ConventionalControllerSettings
            //    .Select(s => s.Assembly)
            //    .Distinct();

            //AddToApplicationParts(partManager, controllerAssemblies);
        }

        private static void AddToApplicationParts(ApplicationPartManager partManager, IEnumerable<Assembly> moduleAssemblies)
        {
            foreach (var moduleAssembly in moduleAssemblies)
            {
                if (partManager.ApplicationParts.Any(p => p is AssemblyPart assemblyPart && assemblyPart.Assembly == moduleAssembly))
                {
                    break;
                }

                partManager.ApplicationParts.Add(new AssemblyPart(moduleAssembly));
            }
        }
    }
}