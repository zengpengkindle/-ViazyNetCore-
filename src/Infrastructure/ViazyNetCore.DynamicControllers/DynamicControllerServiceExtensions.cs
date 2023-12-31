﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;
using ViazyNetCore.Core.System;
using ViazyNetCore.DynamicControllers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DynamicControllerServiceExtensions
    {
        /// <summary>
        /// Use Dynamic WebApi to Configure
        /// </summary>
        /// <param name="application"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDynamicController(this IApplicationBuilder application, Action<IServiceProvider, DynamicControllerOptions>? optionsAction = null)
        {
            var options = new DynamicControllerOptions();

            optionsAction?.Invoke(application.ApplicationServices, options);

            options.Valid();

            DynamicApplicationConsts.DefaultAreaName = options.DefaultAreaName;
            DynamicApplicationConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            DynamicApplicationConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            DynamicApplicationConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            DynamicApplicationConsts.ActionPostfixes = options.RemoveActionPostfixes;
            DynamicApplicationConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            DynamicApplicationConsts.GetRestFulControllerName = options.GetRestFulControllerName;
            DynamicApplicationConsts.GetRestFulActionName = options.GetRestFulActionName;
            DynamicApplicationConsts.AssemblyDynamicApiOptions = options.DynamicAssemblyControllerOptions;

            var partManager = application.ApplicationServices.GetRequiredService<ApplicationPartManager>();

            // Add a custom controller checker
            var featureProviders = application.ApplicationServices.GetRequiredService<DynamicControllerControllerFeatureProvider>();
            partManager.FeatureProviders.Add(featureProviders);

            foreach (var assembly in options.DynamicAssemblyControllerOptions.Keys)
            {
                var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);

                foreach (var part in partFactory.GetApplicationParts(assembly))
                {
                    partManager.ApplicationParts.Add(part);
                }
            }


            var mvcOptions = application.ApplicationServices.GetRequiredService<IOptions<MvcOptions>>();
            var dynamicControllerConvention = application.ApplicationServices.GetRequiredService<DynamicApplicationModelConvention>();

            mvcOptions.Value.Conventions.Add(dynamicControllerConvention);

            return application;
        }


        public static IServiceCollection AddDynamicControllerCore<TSelectController, TActionRouteFactory>(this IServiceCollection services)
            where TSelectController : class, ISelectController
            where TActionRouteFactory : class, IActionRouteFactory
        {
            services.AddSingleton<ISelectController, TSelectController>();
            services.AddSingleton<IActionRouteFactory, TActionRouteFactory>();
            services.AddSingleton<DynamicApplicationModelConvention>();
            services.AddSingleton<DynamicControllerControllerFeatureProvider>();
            return services;
        }

        /// <summary>
        /// Add Dynamic WebApi to Container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddDynamicController(this IServiceCollection services, DynamicControllerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            options.Valid();
            DynamicApplicationConsts.DefaultAreaName = options.DefaultAreaName;
            DynamicApplicationConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            DynamicApplicationConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            DynamicApplicationConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            DynamicApplicationConsts.ActionPostfixes = options.RemoveActionPostfixes;
            DynamicApplicationConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            DynamicApplicationConsts.GetRestFulControllerName = options.GetRestFulControllerName;
            DynamicApplicationConsts.GetRestFulActionName = options.GetRestFulActionName;
            DynamicApplicationConsts.AssemblyDynamicApiOptions = options.DynamicAssemblyControllerOptions;
            //var partManager = (ApplicationPartManager?)services.FirstOrDefault(d => d.ServiceType == typeof(ApplicationPartManager))
            //?.ImplementationInstance;

            //if (partManager == null)
            //{
            //    throw new InvalidOperationException("\"AddDynamicController\" must be after \"AddMvc\".");
            //}
            services.AddSingleton(sp=>options.SelectController);
            services.AddSingleton(sp => options.ActionRouteFactory);

            // Add a custom controller checker
            //partManager.FeatureProviders.Add(new DynamicControllerControllerFeatureProvider(options.SelectController));

            services.Configure<MvcOptions>(o =>
            {
                // Register Controller Routing Information Converter
                //o.Conventions.Add(new DynamicApplicationModelConvention(options.SelectController, options.ActionRouteFactory));
                o.Conventions.Add(new DynamicApplicationModelConvention(options.SelectController, options.ActionRouteFactory));
            });

            return services;
        }

        public static IServiceCollection AddDynamicController(this IServiceCollection services)
        {
            return AddDynamicController(services, new DynamicControllerOptions());
        }

        public static IServiceCollection AddDynamicController(this IServiceCollection services, Action<DynamicControllerOptions> optionsAction)
        {
            var options = new DynamicControllerOptions();

            optionsAction?.Invoke(options);

            return AddDynamicController(services, options);
        }
    }
}
