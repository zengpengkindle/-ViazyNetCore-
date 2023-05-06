using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Modules
{
    public static class PlugInSourceExtensions
    {
        public static Type[] GetModulesWithAllDependencies([NotNull] this IPlugInSource plugInSource)
        {
            Check.NotNull(plugInSource, nameof(plugInSource));

            return plugInSource
                .GetModules()
                .SelectMany(type => ModuleHelper.FindAllModuleTypes(type))
                .Distinct()
                .ToArray();
        }
    }
}
