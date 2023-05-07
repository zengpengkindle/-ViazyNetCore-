using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Identity
{
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
