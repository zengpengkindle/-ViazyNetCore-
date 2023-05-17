using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Attributes;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.MultiTenancy
{
    [Injection(Lifetime = ServiceLifetime.Transient)]
    public class TenantResolver : ITenantResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TenantResolveOptions _options;

        public TenantResolver(IOptions<TenantResolveOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }


        public virtual async Task<TenantResolveResult> ResolveTenantIdOrNameAsync()
        {
            var result = new TenantResolveResult();

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = new TenantResolveContext(serviceScope.ServiceProvider);

                foreach (var tenantResolver in _options.TenantResolvers)
                {
                    await tenantResolver.ResolveAsync(context);

                    result.AppliedResolvers.Add(tenantResolver.Name);

                    if (context.HasResolvedTenantOrHost())
                    {
                        result.TenantIdOrName = context.TenantIdOrName;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
