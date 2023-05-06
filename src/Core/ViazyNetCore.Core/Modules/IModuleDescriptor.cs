using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    public interface IModuleDescriptor
    {

        Type Type { get; }

        Assembly Assembly { get; }

        IInjectionModule Instance { get; }

        bool IsLoadedAsPlugIn { get; }

        IReadOnlyList<IModuleDescriptor> Dependencies { get; }
    }
}
