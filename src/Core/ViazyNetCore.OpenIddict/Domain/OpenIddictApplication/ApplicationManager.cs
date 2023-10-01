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
    public class ApplicationManager : OpenIddictApplicationManager<OpenIddictApplicationDto>, IApplicationManager
    {
        public ApplicationManager(IOpenIddictApplicationCache<OpenIddictApplicationDto> cache
            , ILogger<OpenIddictApplicationManager<OpenIddictApplicationDto>> logger
            , IOptionsMonitor<OpenIddictCoreOptions> options
            , IOpenIddictApplicationStoreResolver resolver)
            : base(cache, logger, options, resolver)
        {
        }

        public async override ValueTask UpdateAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken = default)
        {
            if (!Options.CurrentValue.DisableEntityCaching)
            {
                var entity = await Store.FindByIdAsync(application.Id.ToString(), cancellationToken);
                if (entity != null)
                {
                    await Cache.RemoveAsync(entity, cancellationToken);
                }
            }

            await base.UpdateAsync(application, cancellationToken);
        }

        public async override ValueTask PopulateAsync(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationDto application, CancellationToken cancellationToken = default)
        {
            await base.PopulateAsync(descriptor, application, cancellationToken);

            if (descriptor is ApplicationDescriptor model)
            {
                application.ClientUri = model.ClientUri;
                application.LogoUri = model.LogoUri;
            }
        }

        public async override ValueTask PopulateAsync(OpenIddictApplicationDto application, OpenIddictApplicationDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            await base.PopulateAsync(application, descriptor, cancellationToken);

            if (descriptor is ApplicationDescriptor model)
            {
                application.ClientUri = model.ClientUri;
                application.LogoUri = model.LogoUri;
            }
        }

        public async ValueTask<string> GetClientUriAsync(object application, CancellationToken cancellationToken = default)
        {
            Check.NotNull(application, nameof(application));
            Check.AssignableTo<IOpenIdApplicationStore>(application.GetType(), nameof(application));

            return await Store.As<IOpenIdApplicationStore>().GetClientUriAsync(application.As<OpenIddictApplicationDto>(), cancellationToken);
        }

        public async ValueTask<string> GetLogoUriAsync(object application, CancellationToken cancellationToken = default)
        {
            Check.NotNull(application, nameof(application));
            Check.AssignableTo<IOpenIdApplicationStore>(application.GetType(), nameof(application));

            return await Store.As<IOpenIdApplicationStore>().GetLogoUriAsync(application.As<OpenIddictApplicationDto>(), cancellationToken);
        }
        public override ValueTask<OpenIddictApplicationDto> CreateAsync(OpenIddictApplicationDescriptor descriptor, CancellationToken cancellationToken = default)
        {
            return base.CreateAsync(descriptor, cancellationToken);
        }

        public override ValueTask CreateAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken = default)
        {
            return base.CreateAsync(application, cancellationToken);
        }

        public override ValueTask CreateAsync(OpenIddictApplicationDto application, string? secret, CancellationToken cancellationToken = default)
        {
            return base.CreateAsync(application, secret, cancellationToken);
        }
    }
}
