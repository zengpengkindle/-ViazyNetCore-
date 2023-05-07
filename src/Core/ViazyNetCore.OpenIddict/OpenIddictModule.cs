using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Identity;

namespace ViazyNetCore.OpenIddict
{
    [DependsOn(typeof(IdentityModule))]
    public class OpenIddictModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AutoMapperOptions>(options => options.AddMaps<OpenIddictModule>());
            context.Services.ConfigureOpenIddictServices();
        }
    }
}
