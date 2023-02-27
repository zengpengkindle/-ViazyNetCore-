using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthorizationExtensions
    {
        public static void AddAuthorizationController(this IServiceCollection services)
        {
            services.AddDynamicController(options =>
            {
                options.AddAssemblyOptions(typeof(DynamicControllerBase).Assembly);
            });
        }
    }
}
