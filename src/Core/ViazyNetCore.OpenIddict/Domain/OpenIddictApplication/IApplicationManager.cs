using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace ViazyNetCore.OpenIddict.Domain
{
    public interface IApplicationManager : IOpenIddictApplicationManager
    {
        ValueTask<string> GetClientUriAsync(object application, CancellationToken cancellationToken = default);

        ValueTask<string> GetLogoUriAsync(object application, CancellationToken cancellationToken = default);
    }
}
