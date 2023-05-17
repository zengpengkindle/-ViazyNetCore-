using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.MultiTenancy;

namespace Microsoft.AspNetCore.Builder
{
    public static class AspNetCoreMultiTenancyApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<MultiTenancyMiddleware>();
        }
    }
}
