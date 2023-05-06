using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules;

namespace ViazyNetCore
{
    public interface IModuleLifecycleContributor
    {
        Task InitializeAsync(ApplicationInitializationContext context, IInjectionModule module);

        void Initialize(ApplicationInitializationContext context, IInjectionModule module);

        Task ShutdownAsync(ApplicationShutdownContext context, IInjectionModule module);

        void Shutdown(ApplicationShutdownContext context, IInjectionModule module);
    }
}
