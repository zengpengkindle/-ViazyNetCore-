using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class AuthorizationManager : OpenIddictAuthorizationManager<OpenIddictAuthorizationDto>
    {
        public AuthorizationManager(IOpenIddictAuthorizationCache<OpenIddictAuthorizationDto> cache
            , ILogger<OpenIddictAuthorizationManager<OpenIddictAuthorizationDto>> logger
            , IOptionsMonitor<OpenIddictCoreOptions> options
            , IOpenIddictAuthorizationStoreResolver resolver) : base(cache, logger, options, resolver)
        {
        }

        public async override ValueTask UpdateAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken = default)
        {
            if (!Options.CurrentValue.DisableEntityCaching)
            {
                var entity = await Store.FindByIdAsync(authorization.Id.ToString(), cancellationToken);
                if (entity != null)
                {
                    await Cache.RemoveAsync(entity, cancellationToken);
                }
            }

            await base.UpdateAsync(authorization, cancellationToken);
        }
    }
}
