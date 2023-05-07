using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Modules;

namespace ViazyNetCore
{
    public class InjectionModule :
        IInjectionModule,
        IPostConfigureServices,
        IPreConfigureServices,
        IOnApplicationInitialization,
        IOnApplicationShutdown,
        IOnPreApplicationInitialization,
        IOnPostApplicationInitialization
    {
        private ServiceConfigurationContext? _serviceConfigurationContext;
        protected internal ServiceConfigurationContext ServiceConfigurationContext
        {
            get
            {
                if (_serviceConfigurationContext == null)
                {
                    throw new ApiException($"{nameof(ServiceConfigurationContext)} is only available in the {nameof(ConfigureServices)}, {nameof(PreConfigureServices)} and {nameof(PostConfigureServices)} methods.");
                }

                return _serviceConfigurationContext;
            }
            internal set => _serviceConfigurationContext = value;
        }

        public virtual bool SkipAutoServiceRegistration { get; }

        public virtual void ConfigureServices(ServiceConfigurationContext context)
        {
        }

        public virtual Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            ConfigureServices(context);
            return Task.CompletedTask;
        }

        #region Extension

        public static bool IsInjectionModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IInjectionModule).GetTypeInfo().IsAssignableFrom(type);
        }

        internal static void CheckModuleType(Type moduleType)
        {
            if (!IsInjectionModule(moduleType))
            {
                throw new ArgumentException("Given type is not an ABP module: " + moduleType.AssemblyQualifiedName);
            }
        }

        public Task PostConfigureServicesAsync(ServiceConfigurationContext context)
        {
            PostConfigureServices(context);
            return Task.CompletedTask;
        }

        public virtual void PostConfigureServices(ServiceConfigurationContext context)
        {
        }

        public virtual Task PreConfigureServicesAsync(ServiceConfigurationContext context)
        {
            PreConfigureServices(context);
            return Task.CompletedTask;
        }

        public virtual void PreConfigureServices(ServiceConfigurationContext context)
        {
        }
        #endregion

        #region Configure
        protected void Configure<TOptions>(Action<TOptions> configureOptions)
       where TOptions : class
        {
            ServiceConfigurationContext.Services.Configure(configureOptions);
        }

        protected void Configure<TOptions>(string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            ServiceConfigurationContext.Services.Configure(name, configureOptions);
        }

        protected void Configure<TOptions>(IConfiguration configuration)
            where TOptions : class
        {
            ServiceConfigurationContext.Services.Configure<TOptions>(configuration);
        }

        protected void Configure<TOptions>(IConfiguration configuration, Action<BinderOptions> configureBinder)
            where TOptions : class
        {
            ServiceConfigurationContext.Services.Configure<TOptions>(configuration, configureBinder);
        }

        protected void Configure<TOptions>(string name, IConfiguration configuration)
            where TOptions : class
        {
            ServiceConfigurationContext.Services.Configure<TOptions>(name, configuration);
        }

        protected void PreConfigure<TOptions>(Action<TOptions> configureOptions)
            where TOptions : class
        {
            ServiceConfigurationContext.Services.PreConfigure(configureOptions);
        }

        protected void PostConfigure<TOptions>(Action<TOptions> configureOptions)
            where TOptions : class
        {
            ServiceConfigurationContext.Services.PostConfigure(configureOptions);
        }

        protected void PostConfigureAll<TOptions>(Action<TOptions> configureOptions)
            where TOptions : class
        {
            ServiceConfigurationContext.Services.PostConfigureAll(configureOptions);
        }
        #endregion
        #region Application Initialization

        public virtual Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context)
        {
            OnApplicationInitialization(context);
            return Task.CompletedTask;
        }

        public virtual void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
        }

        public virtual Task OnApplicationShutdownAsync([NotNull] ApplicationShutdownContext context)
        {
            OnApplicationShutdown(context);
            return Task.CompletedTask;
        }

        public virtual void OnApplicationShutdown([NotNull] ApplicationShutdownContext context)
        {
        }

        public virtual Task OnPreApplicationInitializationAsync([NotNull] ApplicationInitializationContext context)
        {
            OnPreApplicationInitialization(context);
            return Task.CompletedTask;
        }

        public virtual void OnPreApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
        }

        public virtual Task OnPostApplicationInitializationAsync([NotNull] ApplicationInitializationContext context)
        {
            OnPostApplicationInitialization(context);
            return Task.CompletedTask;
        }

        public virtual void OnPostApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
        }
        #endregion
    }
}
