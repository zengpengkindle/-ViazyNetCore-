using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.DI;

namespace ViazyNetCore.Authorization
{
    public class AuthorizationModule : InjectionModule
    {
        public AuthorizationModule()
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<MvcOptions>(options =>
            {
            });
            context.Services.AddDynamicController(options =>
            {
                options.AddAssemblyOptions(typeof(DynamicControllerBase).Assembly);
            });
        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            base.PreConfigureServices(context);
        }
    }
}
