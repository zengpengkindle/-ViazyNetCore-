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
    public class ApplicationManager : OpenIddictApplicationManager<OpenIddictApplicationDto>
    {
        public ApplicationManager(IOpenIddictApplicationCache<OpenIddictApplicationDto> cache, ILogger<OpenIddictApplicationManager<OpenIddictApplicationDto>> logger, IOptionsMonitor<OpenIddictCoreOptions> options, IOpenIddictApplicationStoreResolver resolver) 
            : base(cache, logger, options, resolver)
        {
        }
    }
}
