using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.AspNetCore
{
    public class OpenIddictClaimDestinationsProviderContext
    {
        public IServiceProvider ScopeServiceProvider { get; }

        public ClaimsPrincipal Principal { get; }

        public Claim[] Claims { get; }

        public OpenIddictClaimDestinationsProviderContext(IServiceProvider scopeServiceProvider, ClaimsPrincipal principal, Claim[] claims)
        {
            ScopeServiceProvider = scopeServiceProvider;
            Principal = principal;
            Claims = claims;
        }
    }
}
