using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Modules
{
    public class ApplicationWithInternalServiceProvider : ApplicationBase, IApplicationWithInternalServiceProvider
    {
        public IServiceScope ServiceScope { get; private set; }

        public ApplicationWithInternalServiceProvider(
            [NotNull] Type startupModuleType,
           Action<ApplicationCreationOptions>? optionsAction
            ) : this(
            startupModuleType,
            new ServiceCollection(),
            optionsAction)
        {

        }

        private ApplicationWithInternalServiceProvider(
        [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            Action<ApplicationCreationOptions>? optionsAction
            ) : base(
                startupModuleType,
                services,
                optionsAction)
        {
            Services.AddSingleton<IApplicationWithInternalServiceProvider>(this);
        }

        public IServiceProvider CreateServiceProvider()
        {
            if (ServiceProvider != null)
            {
                return ServiceProvider;
            }

            ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
            SetServiceProvider(ServiceScope.ServiceProvider);

            return ServiceProvider;
        }

        public async Task InitializeAsync()
        {
            CreateServiceProvider();
            await InitializeModulesAsync();
        }

        public void Initialize()
        {
            CreateServiceProvider();
            InitializeModules();
        }

        public override void Dispose()
        {
            base.Dispose();
            ServiceScope?.Dispose();
        }
    }

}
