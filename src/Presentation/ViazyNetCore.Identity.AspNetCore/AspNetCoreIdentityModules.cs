using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.AutoMapper;

namespace ViazyNetCore.Identity.AspNetCore
{
    [DependsOn(
        typeof(IdentityAuthModule)
        , typeof(AspNetCoreModule)
        , typeof(AutoMapperModule))]
    public class AspNetCoreIdentityModules : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
