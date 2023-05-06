using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Modules
{
    public static class ApplicationFactory
    {
        public async static Task<IApplicationWithExternalServiceProvider> CreateAsync(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        Action<ApplicationCreationOptions> optionsAction = null)
        {
            var app = new ApplicationWithExternalServiceProvider(startupModuleType, services, options =>
            {
                options.SkipConfigureServices = true;
                optionsAction?.Invoke(options);
            });
            await app.ConfigureServicesAsync();
            return app;
        }

        public async static Task<IApplicationWithExternalServiceProvider> CreateAsync<TStartupModule>(
         [NotNull] IServiceCollection services,
        Action<ApplicationCreationOptions> optionsAction = null)
         where TStartupModule : IInjectionModule
        {
            var app = Create(typeof(TStartupModule), services, options =>
            {
                options.SkipConfigureServices = true;
                optionsAction?.Invoke(options);
            });
            await app.ConfigureServicesAsync();
            return app;
        }

        public static IApplicationWithExternalServiceProvider Create<TStartupModule>(
    [NotNull] IServiceCollection services,
     Action<ApplicationCreationOptions> optionsAction = null)
    where TStartupModule : IInjectionModule
        {
            return Create(typeof(TStartupModule), services, optionsAction);
        }

        public static IApplicationWithExternalServiceProvider Create(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            Action<ApplicationCreationOptions> optionsAction = null)
        {
            return new ApplicationWithExternalServiceProvider(startupModuleType, services, optionsAction);
        }
    }
}
