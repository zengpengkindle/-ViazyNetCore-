using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Modules
{
    public class ApplicationWithExternalServiceProvider : ApplicationBase, IApplicationWithExternalServiceProvider
    {
        public ApplicationWithExternalServiceProvider([NotNull] Type startupModuleType, [NotNull] IServiceCollection services, Action<ApplicationCreationOptions>? optionsAction) 
            : base(startupModuleType, services, optionsAction)
        {
            services.AddSingleton<IApplicationWithExternalServiceProvider>(this);
        }

        void IApplicationWithExternalServiceProvider.SetServiceProvider([NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            if (ServiceProvider != null)
            {
                if (ServiceProvider != serviceProvider)
                {
                    throw new Exception("Service provider was already set before to another service provider instance.");
                }

                return;
            }

            SetServiceProvider(serviceProvider);
        }

        public async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            SetServiceProvider(serviceProvider);

            await InitializeModulesAsync();
        }

        public void Initialize([NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            SetServiceProvider(serviceProvider);

            InitializeModules();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (ServiceProvider is IDisposable disposableServiceProvider)
            {
                disposableServiceProvider.Dispose();
            }
        }
    }
}
