using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.Modules
{
    [Injection(Lifetime = ServiceLifetime.Singleton)]
    public class ModuleManager : IModuleManager
    {
        private readonly IModuleContainer _moduleContainer;
        private readonly IEnumerable<IModuleLifecycleContributor> _lifecycleContributors;
        private readonly ILogger<ModuleManager> _logger;

        public ModuleManager(
       IModuleContainer moduleContainer,
       ILogger<ModuleManager> logger,
       IOptions<ModuleLifecycleOptions> options,
       IServiceProvider serviceProvider)
        {
            _moduleContainer = moduleContainer;
            _logger = logger;

            _lifecycleContributors = options.Value
                .Contributors
                .Select(serviceProvider.GetRequiredService)
                .Cast<IModuleLifecycleContributor>()
                .ToArray();
        }

        public virtual async Task InitializeModulesAsync(ApplicationInitializationContext context)
        {
            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    try
                    {
                        await contributor.InitializeAsync(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }

            _logger.LogInformation("Initialized all ABP modules.");
        }

        public void InitializeModules(ApplicationInitializationContext context)
        {
            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    try
                    {
                        contributor.Initialize(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }

            _logger.LogInformation("Initialized all ABP modules.");
        }

        public virtual async Task ShutdownModulesAsync(ApplicationShutdownContext context)
        {
            var modules = _moduleContainer.Modules.Reverse().ToList();

            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in modules)
                {
                    try
                    {
                        await contributor.ShutdownAsync(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred during the shutdown {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }
        }

        public void ShutdownModules(ApplicationShutdownContext context)
        {
            var modules = _moduleContainer.Modules.Reverse().ToList();

            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in modules)
                {
                    try
                    {
                        contributor.Shutdown(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred during the shutdown {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }
        }
    }
}
