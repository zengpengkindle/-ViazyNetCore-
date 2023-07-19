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
    public class TokenManager : OpenIddictTokenManager<OpenIddictTokenDto>
    {
        public TokenManager(IOpenIddictTokenCache<OpenIddictTokenDto> cache
            , ILogger<OpenIddictTokenManager<OpenIddictTokenDto>> logger
            , IOptionsMonitor<OpenIddictCoreOptions> options
            , IOpenIddictTokenStoreResolver resolver) : base(cache, logger, options, resolver)
        {
        }

        public async override ValueTask UpdateAsync(OpenIddictTokenDto token, CancellationToken cancellationToken = default)
        {
            if (!Options.CurrentValue.DisableEntityCaching)
            {
                var entity = await Store.FindByIdAsync(token.Id.ToString(), cancellationToken);
                if (entity != null)
                {
                    await Cache.RemoveAsync(entity, cancellationToken);
                }
            }

            await base.UpdateAsync(token, cancellationToken);
        }
    }
}
