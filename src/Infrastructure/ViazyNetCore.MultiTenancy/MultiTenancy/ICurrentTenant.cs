using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.MultiTenancy
{
    [Injection(Lifetime = ServiceLifetime.Scoped)]
    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        int? Id { get; }

        string Name { get; }

        IDisposable Change(int? id, string name = null);
    }
}
