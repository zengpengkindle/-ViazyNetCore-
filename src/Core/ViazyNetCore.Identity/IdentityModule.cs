using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.AutoMapper;

namespace ViazyNetCore.Identity
{
    [DependsOn(typeof(AutoMapperModule))]
    public class IdentityModule : InjectionModule
    {
        public IdentityModule()
        {
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<IdentityModule>());
        }
    }
}
