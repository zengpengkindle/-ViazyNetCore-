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
    public class ScopeManager : OpenIddictScopeManager<OpenIddictScopeDto>
    {
        public ScopeManager(IOpenIddictScopeCache<OpenIddictScopeDto> cache
            , ILogger<OpenIddictScopeManager<OpenIddictScopeDto>> logger
            , IOptionsMonitor<OpenIddictCoreOptions> options
            , IOpenIddictScopeStoreResolver resolver) : base(cache, logger, options, resolver)
        {
        }
        public async override ValueTask UpdateAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken = default)
        {
            if (!Options.CurrentValue.DisableEntityCaching)
            {
                var entity = await Store.FindByIdAsync(scope.Id.ToString(), cancellationToken);
                if (entity != null)
                {
                    await Cache.RemoveAsync(entity, cancellationToken);
                }
            }

            await base.UpdateAsync(scope, cancellationToken);
        }
    }
}
