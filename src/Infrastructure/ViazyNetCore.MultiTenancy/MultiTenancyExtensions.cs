using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.MultiTenancy;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultiTenancyExtensions
    {
        public static void AddMultiTenancy(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);
            services.AddSingleton<ICurrentTenant, CurrentTenant>();
        }
    }
}
