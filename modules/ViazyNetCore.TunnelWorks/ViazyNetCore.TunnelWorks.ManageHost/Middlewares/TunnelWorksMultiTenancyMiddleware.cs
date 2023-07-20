using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Data.FreeSql.Extensions;
using ViazyNetCore.MultiTenancy;

namespace ViazyNetCore.TunnelWorks.ManageHost.Middlewares
{
    public class TunnelWorksMultiTenancyMiddleware : IMiddleware
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly ILogger<TunnelWorksMultiTenancyMiddleware> _logger;
        private readonly IUser _user;

        public TunnelWorksMultiTenancyMiddleware(ICurrentTenant currentTenant
            , ILogger<TunnelWorksMultiTenancyMiddleware> logger
            , IUser user)
        {
            this._currentTenant = currentTenant;
            this._logger = logger;
            this._user = user;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using(_currentTenant.Change(100_001, "TunnelWorks"))
            {
                (_user as User).TenantId = 100_001;
                await next(context);
            }
        }
    }
}
