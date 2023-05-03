using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.MultiTenancy
{
    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        long? Id { get; }

        string Name { get; }

        IDisposable Change(long? id, string name = null);
    }
}
