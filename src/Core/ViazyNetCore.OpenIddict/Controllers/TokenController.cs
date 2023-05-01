using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using ViazyNetCore.OpenIddict.AspNetCore;
using ViazyNetCore.OpenIddict.AspNetCore.ExtensionGrantTypes;
using ViazyNetCore.OpenIddict.Domain;

namespace ViazyNetCore.OpenIddict.Controllers;

[Route("connect/token")]
[IgnoreAntiforgeryToken]
[ApiExplorerSettings(IgnoreApi = true)]
public partial class TokenController : OpenIdDictControllerBase
{
    public TokenController(SignInManager<Domain.IdentityUser> signInManager
        , ILogger<TokenController> logger
        , IOptions<IdentityOptions> options
        , IOpenIddictApplicationManager applicationManager
        , IOpenIddictAuthorizationManager authorizationManager
        , OpenIddictClaimDestinationsManager openIddictClaimDestinationsManager
        , IdentityUserManager userManager
        , IOpenIddictScopeManager scopeManager
        , IOpenIddictTokenManager tokenManager
        , IServiceScopeFactory serviceScopeFactory)
        : base(signInManager, applicationManager, authorizationManager, openIddictClaimDestinationsManager, userManager, scopeManager, tokenManager)
    {
        this.Logger = logger;
        this.IdentityOptions = options;
        this.ServiceScopeFactory = serviceScopeFactory;
    }

    public ILogger<TokenController> Logger { get; }
    public IOptions<IdentityOptions> IdentityOptions { get; }

    [HttpGet, HttpPost, Produces("application/json")]
    public virtual async Task<IActionResult> HandleAsync()
    {
        var request = await GetOpenIddictServerRequestAsync(HttpContext);

        if (request.IsPasswordGrantType())
        {
            return await HandlePasswordAsync(request);
        }

        if (request.IsAuthorizationCodeGrantType())
        {
            return await HandleAuthorizationCodeAsync(request);
        }

        if (request.IsRefreshTokenGrantType())
        {
            return await HandleRefreshTokenAsync(request);
        }

        if (request.IsDeviceCodeGrantType())
        {
            return await HandleDeviceCodeAsync(request);
        }

        if (request.IsClientCredentialsGrantType())
        {
            return await HandleClientCredentialsAsync(request);
        }

        var extensionGrantsOptions = HttpContext.RequestServices.GetRequiredService<IOptions<OpenIddictExtensionGrantsOptions>>();
        var extensionTokenGrant = extensionGrantsOptions.Value.Find<ITokenExtensionGrant>(request.GrantType);
        if (extensionTokenGrant != null)
        {
            return await extensionTokenGrant.HandleAsync(new ExtensionGrantContext(HttpContext, request));
        }

        throw new ApiException(string.Format("TheSpecifiedGrantTypeIsNotImplemented", request.GrantType));
    }
}
