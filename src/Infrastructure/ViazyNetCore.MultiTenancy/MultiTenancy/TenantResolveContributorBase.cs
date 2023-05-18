using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.MultiTenancy
{
    public abstract class TenantResolveContributorBase : ITenantResolveContributor
    {
        public abstract string Name { get; }

        public abstract Task ResolveAsync(ITenantResolveContext context);
    }
}
