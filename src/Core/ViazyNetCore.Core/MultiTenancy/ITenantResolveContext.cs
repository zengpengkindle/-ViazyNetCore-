using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.MultiTenancy
{
    public interface ITenantResolveContext
    {
        IServiceProvider ServiceProvider { get; }

        string TenantIdOrName { get; set; }

        bool Handled { get; set; }
    }
}
