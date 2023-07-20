using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.MultiTenancy;

namespace ViazyNetCore.TunnelWorks.ManageHost.Middlewares
{
    public class MultiTenancyMiddleware : IMiddleware
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly ILogger<MultiTenancyMiddleware> _logger;

        public MultiTenancyMiddleware(ICurrentTenant currentTenant
            , ILogger<MultiTenancyMiddleware> logger)
        {
            this._currentTenant = currentTenant;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using(_currentTenant.Change(100_001, "TunnelWorks"))
            {
                await next(context);
            }
        }
    }
}
