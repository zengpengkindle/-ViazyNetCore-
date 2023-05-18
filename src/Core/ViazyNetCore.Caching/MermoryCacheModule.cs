using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ViazyNetCore.Caching
{
    public class MermoryCacheModule:InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache();
            context.Services.TryAddSingleton<ICache, RuntimeMemoryCache>();
        }
    }
}
