using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ViazyNetCore.MultiTenancy
{
    [DependsOn(typeof(MermoryCacheModule))]
    public class MultiTenancyModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.TryAddTransient<ITenantStore, FreeSqlTenantStore>();
            context.Services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);
        }
    }
}
