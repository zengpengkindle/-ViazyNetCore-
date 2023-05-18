using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore.AspNetCore;

namespace ViazyNetCore.MultiTenancy
{
    [DependsOn(
    typeof(MultiTenancyModule),
    typeof(AspNetCoreModule)
    )]
    public class AspNetCoreMultiTenancyModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<TenantResolveOptions>(options => { });
            //context.Services.AddTransient<ITenantResolver, TenantResolver>();
            //context.Services.AddTransient<ITenantConfigurationProvider, TenantConfigurationProvider>();
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseMultiTenancy();
        }
    }
}
