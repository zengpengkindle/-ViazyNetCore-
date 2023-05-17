using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.MultiTenancy
{
    public interface ITenantStore
    {
        Task<TenantConfiguration> FindAsync(int id);
        Task<TenantConfiguration> FindAsync(string name);
        TenantConfiguration Find(int id);
        TenantConfiguration Find(string name);
    }
}
