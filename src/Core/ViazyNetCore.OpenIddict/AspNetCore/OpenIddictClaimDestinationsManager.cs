using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.OpenIddict.AspNetCore
{
    public class OpenIddictClaimDestinationsManager
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected IOptions<OpenIddictClaimDestinationsOptions> Options { get; }

        public OpenIddictClaimDestinationsManager(IServiceScopeFactory serviceScopeFactory, IOptions<OpenIddictClaimDestinationsOptions> options)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Options = options;
        }

        public virtual async Task SetAsync(ClaimsPrincipal principal)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                foreach (var providerType in Options.Value.ClaimDestinationsProvider)
                {
                    var provider = (IOpenIddictClaimDestinationsProvider)scope.ServiceProvider.GetRequiredService(providerType);
                    await provider.SetDestinationsAsync(new OpenIddictClaimDestinationsProviderContext(scope.ServiceProvider, principal, principal.Claims.ToArray()));
                }
            }
        }
    }
}
