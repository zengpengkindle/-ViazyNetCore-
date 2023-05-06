using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    public class ModuleLifecycleContributorBase : IModuleLifecycleContributor
    {
        public virtual Task InitializeAsync(ApplicationInitializationContext context, IInjectionModule module)
        {
            return Task.CompletedTask;
        }

        public virtual void Initialize(ApplicationInitializationContext context, IInjectionModule module)
        {
        }

        public virtual Task ShutdownAsync(ApplicationShutdownContext context, IInjectionModule module)
        {
            return Task.CompletedTask;
        }

        public virtual void Shutdown(ApplicationShutdownContext context, IInjectionModule module)
        {
        }
    }
}
