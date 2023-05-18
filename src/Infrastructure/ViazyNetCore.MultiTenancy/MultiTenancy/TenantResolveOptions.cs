using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.MultiTenancy
{
    public class TenantResolveOptions
    {
        [NotNull]
        public List<ITenantResolveContributor> TenantResolvers { get; }

        public TenantResolveOptions()
        {
            TenantResolvers = new List<ITenantResolveContributor>
            {
                new CurrentUserTenantResolveContributor()
            };
        }
    }
}
