using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.AspNetCore
{
    public interface IOpenIddictClaimDestinationsProvider
    {
        Task SetDestinationsAsync(OpenIddictClaimDestinationsProviderContext context);
    }
}
