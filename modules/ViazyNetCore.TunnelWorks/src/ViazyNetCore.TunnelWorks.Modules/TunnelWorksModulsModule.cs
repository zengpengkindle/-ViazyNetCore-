using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.AutoMapper;

namespace ViazyNetCore.TunnelWorks.Modules
{
    [DependsOn(typeof(AutoMapperModule))]
    public class TunnelWorksModulsModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<TunnelWorksModulsModule>());
        }
    }
}
