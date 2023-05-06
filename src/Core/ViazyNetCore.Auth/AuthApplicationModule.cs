using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.Auth
{
    public class AuthApplicationModule : InjectionModule
    {
        public AuthApplicationModule()
        {

        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddJwtAuthentication(options =>
            {
            });
        }
    }
}
