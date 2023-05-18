using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace ViazyNetCore.MultiTenancy
{
    [Injection(Lifetime = ServiceLifetime.Transient)]
    public class TenantConfigurationProvider : ITenantConfigurationProvider
    {
        protected virtual ITenantStore TenantStore { get; }
        protected virtual ITenantResolver TenantResolver { get; }

        public TenantConfigurationProvider(ITenantStore tenantStore, ITenantResolver tenantResolver)
        {
            this.TenantStore = tenantStore;
            this.TenantResolver = tenantResolver;
        }

        public virtual async Task<TenantConfiguration> GetAsync(bool saveResolveResult = false)
        {
            var resolveResult = await TenantResolver.ResolveTenantIdOrNameAsync();

            //if (saveResolveResult)
            //{
            //    TenantResolveResultAccessor.Result = resolveResult;
            //}

            TenantConfiguration tenant = null;
            if (resolveResult.TenantIdOrName != null)
            {
                tenant = await FindTenantAsync(resolveResult.TenantIdOrName);

                if (tenant == null)
                {
                    throw new ApiException(
                        statusCode: 10001,
                        message: "Tenant not found!"
                    );
                }

                if (!tenant.IsActive)
                {
                    throw new ApiException(
                        statusCode: 10002,
                        message: "Tenant not active!"
                    );
                }
            }

            return tenant;
        }

        protected virtual async Task<TenantConfiguration> FindTenantAsync(string tenantIdOrName)
        {
            if (int.TryParse(tenantIdOrName, out var parsedTenantId))
            {
                return await TenantStore.FindAsync(parsedTenantId);
            }
            else
            {
                return await TenantStore.FindAsync(tenantIdOrName);
            }
        }
    }
}
