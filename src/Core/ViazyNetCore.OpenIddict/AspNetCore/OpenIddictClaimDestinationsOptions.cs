using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.AspNetCore
{
    public class OpenIddictClaimDestinationsOptions
    {
        public ITypeList<IOpenIddictClaimDestinationsProvider> ClaimDestinationsProvider { get; }

        public OpenIddictClaimDestinationsOptions()
        {
            ClaimDestinationsProvider = new TypeList<IOpenIddictClaimDestinationsProvider>();
        }
    }
}
