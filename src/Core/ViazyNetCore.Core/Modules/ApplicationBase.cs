using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ViazyNetCore.DependencyInjection;

namespace ViazyNetCore.Modules
{
    public abstract class ApplicationBase : IApplication
    {
        public Type StartupModuleType { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IServiceCollection Services { get; }

        public IReadOnlyList<IModuleDescriptor> Modules { get; }

        public string ApplicationName { get; }

        public string InstanceId { get; } = Guid.NewGuid().ToString();
        private bool _configuredServices;

        internal ApplicationBase(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            Action<ApplicationCreationOptions>? optionsAction)
        {
            Check.NotNull(startupModuleType, nameof(startupModuleType));
            Check.NotNull(services, nameof(services));

            StartupModuleType = startupModuleType;
            Services = services;

            services.TryAddObjectAccessor<IServiceProvider>();

            var options = new ApplicationCreationOptions(services);
            optionsAction?.Invoke(options);

            ApplicationName = GetApplicationName(options);

            services.AddSingleton<IApplication>(this);
            services.AddSingleton<IApplicationInfoAccessor>(this);
            services.AddSingleton<IModuleContainer>(this);
            //services.AddSingleton<IHostEnvironment>();

            services.AddCoreServices();
            services.AddApplicationCoreServices(this, options);

            Modules = LoadModules(services, options);

            if (!options.SkipConfigureServices)
            {
                ConfigureServices();
            }
        }

        public virtual async Task ShutdownAsync()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .ShutdownModulesAsync(new ApplicationShutdownContext(scope.ServiceProvider));
            }
        }

        public virtual void Shutdown()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .ShutdownModules(new ApplicationShutdownContext(scope.ServiceProvider));
            }
        }

        public virtual void Dispose()
        {
            //TODO: Shutdown if not done before?
        }

        protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
        }

        protected virtual async Task InitializeModulesAsync()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                WriteInitLogs(scope.ServiceProvider);
                await scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .InitializeModulesAsync(new ApplicationInitializationContext(scope.ServiceProvider));
            }
        }

        protected virtual void InitializeModules()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                WriteInitLogs(scope.ServiceProvider);
                scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .InitializeModules(new ApplicationInitializationContext(scope.ServiceProvider));
            }
        }

        protected virtual void WriteInitLogs(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<ApplicationBase>>();
            if (logger == null)
            {
                return;
            }

            //var initLogger = serviceProvider.GetRequiredService<IInitLoggerFactory>().Create<ApplicationBase>();

            //foreach (var entry in initLogger.Entries)
            //{
            //    logger.Log(entry.LogLevel, entry.EventId, entry.State, entry.Exception, entry.Formatter);
            //}

            //initLogger.Entries.Clear();
        }

        protected virtual IReadOnlyList<IModuleDescriptor> LoadModules(IServiceCollection services, ApplicationCreationOptions options)
        {
            return services
                .GetSingletonInstance<IModuleLoader>()
                .LoadModules(
                    services,
                    StartupModuleType,
                    options.PlugInSources
                );
        }

        //TODO: We can extract a new class for this
        public virtual async Task ConfigureServicesAsync()
        {
            CheckMultipleConfigureServices();

            var context = new ServiceConfigurationContext(Services);
            Services.AddSingleton(context);

            foreach (var module in Modules)
            {
                if (module.Instance is InjectionModule InjectionModule)
                {
                    InjectionModule.ServiceConfigurationContext = context;
                }
            }

            //PreConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
            {
                try
                {
                    await ((IPreConfigureServices)module.Instance).PreConfigureServicesAsync(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IPreConfigureServices.PreConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            var assemblies = new HashSet<Assembly>();

            //ConfigureServices
            foreach (var module in Modules)
            {
                if (module.Instance is InjectionModule injectionModule)
                {
                    if (!injectionModule.SkipAutoServiceRegistration)
                    {
                        var assembly = module.Type.Assembly;
                        if (!assemblies.Contains(assembly))
                        {
                            Services.AddAssembly(assembly);
                            assemblies.Add(assembly);
                        }
                    }
                }

                try
                {
                    await module.Instance.ConfigureServicesAsync(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IInjectionModule.ConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            //PostConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPostConfigureServices))
            {
                try
                {
                    await ((IPostConfigureServices)module.Instance).PostConfigureServicesAsync(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IPostConfigureServices.PostConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            foreach (var module in Modules)
            {
                if (module.Instance is InjectionModule injectionModule)
                {
                    injectionModule.ServiceConfigurationContext = null;
                }
            }

            _configuredServices = true;

            TryToSetEnvironment(Services);
        }

        private void CheckMultipleConfigureServices()
        {
            if (_configuredServices)
            {
                throw new Exception("Services have already been configured! If you call ConfigureServicesAsync method, you must have set ApplicationCreationOptions.SkipConfigureServices to true before.");
            }
        }

        //TODO: We can extract a new class for this
        public virtual void ConfigureServices()
        {
            CheckMultipleConfigureServices();

            var context = new ServiceConfigurationContext(Services);
            Services.AddSingleton(context);

            foreach (var module in Modules)
            {
                if (module.Instance is InjectionModule InjectionModule)
                {
                    InjectionModule.ServiceConfigurationContext = context;
                }
            }

            //PreConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
            {
                try
                {
                    ((IPreConfigureServices)module.Instance).PreConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IPreConfigureServices.PreConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            var assemblies = new HashSet<Assembly>();

            //ConfigureServices
            foreach (var module in Modules)
            {
                if (module.Instance is InjectionModule InjectionModule)
                {
                    if (!InjectionModule.SkipAutoServiceRegistration)
                    {
                        var assembly = module.Type.Assembly;
                        if (!assemblies.Contains(assembly))
                        {
                            Services.AddAssembly(assembly);
                            assemblies.Add(assembly);
                        }
                    }
                }

                try
                {
                    module.Instance.ConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IInjectionModule.ConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            //PostConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPostConfigureServices))
            {
                try
                {
                    ((IPostConfigureServices)module.Instance).PostConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IPostConfigureServices.PostConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            foreach (var module in Modules)
            {
                if (module.Instance is InjectionModule InjectionModule)
                {
                    InjectionModule.ServiceConfigurationContext = null;
                }
            }

            _configuredServices = true;

            TryToSetEnvironment(Services);
        }

        private static string GetApplicationName(ApplicationCreationOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.ApplicationName))
            {
                return options.ApplicationName;
            }

            var configuration = options.Services.GetConfigurationOrNull();
            if (configuration != null)
            {
                var appNameConfig = configuration["ApplicationName"];
                if (!string.IsNullOrWhiteSpace(appNameConfig))
                {
                    return appNameConfig;
                }
            }

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                return entryAssembly.GetName().Name;
            }

            return null;
        }

        private static void TryToSetEnvironment(IServiceCollection services)
        {
            //var hostEnvironment = services.GetSingletonInstance<IHostEnvironment>();
            //if (hostEnvironment.EnvironmentName.IsNullOrWhiteSpace())
            //{
            //    hostEnvironment.EnvironmentName = Environments.Production;
            //}
        }
    }
}
