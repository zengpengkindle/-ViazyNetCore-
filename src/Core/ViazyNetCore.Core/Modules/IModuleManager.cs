using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    public interface IModuleManager
    {
        Task InitializeModulesAsync([NotNull] ApplicationInitializationContext context);

        void InitializeModules([NotNull] ApplicationInitializationContext context);

        Task ShutdownModulesAsync([NotNull] ApplicationShutdownContext context);

        void ShutdownModules([NotNull] ApplicationShutdownContext context);
    }
}
