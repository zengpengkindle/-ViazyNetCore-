using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;
using ViazyNetCore.Core.System;
using ViazyNetCore.DI;

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

            AppConsts.DefaultAreaName = options.DefaultAreaName;
            AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
            AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            AppConsts.GetRestFulControllerName = options.GetRestFulControllerName;
            AppConsts.GetRestFulActionName = options.GetRestFulActionName;
            AppConsts.AssemblyDynamicApiOptions = options.DynamicAssemblyControllerOptions;

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
            var dynamicControllerConvention = application.ApplicationServices.GetRequiredService<DynamicControllerConvention>();

            mvcOptions.Value.Conventions.Add(dynamicControllerConvention);

            return application;
        }


        public static IServiceCollection AddDynamicControllerCore<TSelectController, TActionRouteFactory>(this IServiceCollection services)
            where TSelectController : class, ISelectController
            where TActionRouteFactory : class, IActionRouteFactory
        {
            services.AddSingleton<ISelectController, TSelectController>();
            services.AddSingleton<IActionRouteFactory, TActionRouteFactory>();
            services.AddSingleton<DynamicControllerConvention>();
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

            AppConsts.DefaultAreaName = options.DefaultAreaName;
            AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
            AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            AppConsts.GetRestFulControllerName = options.GetRestFulControllerName;
            AppConsts.GetRestFulActionName = options.GetRestFulActionName;
            AppConsts.AssemblyDynamicApiOptions = options.DynamicAssemblyControllerOptions;
            var partManager = (ApplicationPartManager?)services.FirstOrDefault(d => d.ServiceType == typeof(ApplicationPartManager))
            ?.ImplementationInstance;

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddDynamicController\" must be after \"AddMvc\".");
            }

            // Add a custom controller checker
            partManager.FeatureProviders.Add(new DynamicControllerControllerFeatureProvider(options.SelectController));

            services.Configure<MvcOptions>(o =>
            {
                // Register Controller Routing Information Converter
                o.Conventions.Add(new DynamicControllerConvention(options.SelectController, options.ActionRouteFactory));
            });

            return services;
        }

        public static IServiceCollection AddDynamicController(this IServiceCollection services)
        {
            return AddDynamicController(services, new DynamicControllerOptions());
        }

        public static IServiceCollection AddDynamicController(this IServiceCollection services, Action<DynamicControllerOptions> optionsAction)
        {
            var DynamicApiOptions = new DynamicControllerOptions();

            optionsAction?.Invoke(DynamicApiOptions);

            return AddDynamicController(services, DynamicApiOptions);
        }
    }
}
