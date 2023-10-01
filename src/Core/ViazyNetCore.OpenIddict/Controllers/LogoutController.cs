using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using ViazyNetCore.Identity.Domain;
using ViazyNetCore.OpenIddict.AspNetCore;
using ViazyNetCore.OpenIddict.Domain;

namespace ViazyNetCore.OpenIddict.Controllers;

[Route("connect/logout")]
[ApiExplorerSettings(IgnoreApi = true)]
public class LogoutController : OpenIdDictControllerBase
{
    public LogoutController(SignInManager<Identity.Domain.IdentityUser> signInManager
        , IOpenIddictApplicationManager applicationManager
        , IOpenIddictAuthorizationManager authorizationManager
        , OpenIddictClaimDestinationsManager openIddictClaimDestinationsManager
        , IdentityUserManager userManager
        , IOpenIddictScopeManager scopeManager
        , IOpenIddictTokenManager tokenManager)
        : base(signInManager, applicationManager, authorizationManager, openIddictClaimDestinationsManager, userManager, scopeManager, tokenManager)
    {
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAsync()
    {
        // Ask ASP.NET Core Identity to delete the local and external cookies created
        // when the user agent is redirected from the external identity provider
        // after a successful authentication flow (e.g Google or Facebook).
        await SignInManager.SignOutAsync();

        // Returning a SignOutResult will ask OpenIddict to redirect the user agent
        // to the post_logout_redirect_uri specified by the client application or to
        // the RedirectUri specified in the authentication properties if none was set.
        return SignOut(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
