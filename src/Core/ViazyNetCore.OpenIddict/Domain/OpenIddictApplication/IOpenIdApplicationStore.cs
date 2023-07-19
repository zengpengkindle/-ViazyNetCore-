using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace ViazyNetCore.OpenIddict.Domain
{
    public interface IOpenIdApplicationStore : IOpenIddictApplicationStore<OpenIddictApplicationDto>
    {
        ValueTask<string> GetClientUriAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken = default);

        ValueTask<string> GetLogoUriAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken = default);
    }
}
