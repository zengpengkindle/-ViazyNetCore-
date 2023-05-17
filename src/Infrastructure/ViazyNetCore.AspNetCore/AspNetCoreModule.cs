using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Modules;

namespace ViazyNetCore.AspNetCore
{
    public class AspNetCoreModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddObjectAccessor<IApplicationBuilder>();

            services.AddHttpContextAccessor();
            //services.AddScoped<ICurrentUser, HttpContextCurrentUser>();
        }
    }
}
