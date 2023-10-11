using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.Identity;

namespace ViazyNetCore.CmsKit.Domain
{
    [DependsOn(typeof(AutoMapperModule),
        typeof(IdentityAuthModule))]
    public class CmsKitDomainModules : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<CmsKitDomainModules>());
        }
    }
}
