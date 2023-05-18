using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.MultiTenancy
{
    public class MultiTenancyMiddleware : IMiddleware
    {
        private readonly ITenantConfigurationProvider _tenantConfigurationProvider;
        private readonly ICurrentTenant _currentTenant;

        public MultiTenancyMiddleware(ITenantConfigurationProvider tenantConfigurationProvider
            , ICurrentTenant currentTenant
            , ILogger<MultiTenancyMiddleware> logger)
        {
            this._tenantConfigurationProvider = tenantConfigurationProvider;
            this._currentTenant = currentTenant;
            this.Logger = logger;
        }

        public ILogger<MultiTenancyMiddleware> Logger { get; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            TenantConfiguration tenant;
            try
            {
                tenant = await _tenantConfigurationProvider.GetAsync(saveResolveResult: true);
            }
            catch (Exception e)
            {
                Logger.LogError("Tenant Error", e);
                throw new ApiException("Tenant Error", 500);
            }
            if (tenant?.Id != _currentTenant.Id)
            {
                using (_currentTenant.Change(tenant?.Id, tenant?.Name))
                {
                    await next(context);
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}
