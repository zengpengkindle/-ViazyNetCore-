using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using ViazyNetCore.OpenIddict.AspNetCore;
using ViazyNetCore.OpenIddict.Domain;
using Microsoft.AspNetCore.Identity;
using IdentityUser = ViazyNetCore.OpenIddict.Domain.IdentityUser;

namespace ViazyNetCore.OpenIddict.Controllers
{
    [ControllerGroup("openiddict")]
    public class OpenIdDictControllerBase : Controller
    {
        protected SignInManager<IdentityUser> SignInManager { get; }
        protected IOpenIddictApplicationManager ApplicationManager { get; }
        protected IOpenIddictAuthorizationManager AuthorizationManager { get; }
        protected IOpenIddictScopeManager ScopeManager { get; }
        protected IOpenIddictTokenManager TokenManager { get; }
        protected IdentityUserManager UserManager { get; }
        protected OpenIddictClaimDestinationsManager OpenIddictClaimDestinationsManager { get; }
        public OpenIdDictControllerBase(SignInManager<IdentityUser> signInManager
            , IOpenIddictApplicationManager applicationManager
            , IOpenIddictAuthorizationManager authorizationManager
            , OpenIddictClaimDestinationsManager openIddictClaimDestinationsManager
            , IdentityUserManager userManager
            , IOpenIddictScopeManager scopeManager
            , IOpenIddictTokenManager tokenManager)
        {
            this.SignInManager = signInManager;
            this.ApplicationManager = applicationManager;
            this.AuthorizationManager = authorizationManager;
            this.OpenIddictClaimDestinationsManager = openIddictClaimDestinationsManager;
            this.UserManager = userManager;
            this.ScopeManager = scopeManager;
            this.TokenManager = tokenManager;
        }
        protected virtual Task<OpenIddictRequest> GetOpenIddictServerRequestAsync(HttpContext httpContext)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect Request Cannot Be Retrieved");

            return Task.FromResult(request);
        }

        protected virtual async Task<IEnumerable<string>> GetResourcesAsync(ImmutableArray<string> scopes)
        {
            var resources = new List<string>();
            if (!scopes.Any())
            {
                return resources;
            }

            await foreach (var resource in ScopeManager.ListResourcesAsync(scopes))
            {
                resources.Add(resource);
            }
            return resources;
        }

        protected virtual async Task SetClaimsDestinationsAsync(ClaimsPrincipal principal)
        {
            await OpenIddictClaimDestinationsManager.SetAsync(principal);
        }

        protected virtual async Task<bool> HasFormValueAsync(string name)
        {
            if (Request.HasFormContentType)
            {
                var form = await Request.ReadFormAsync();
                if (!string.IsNullOrEmpty(form[name]))
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual async Task<bool> PreSignInCheckAsync(IdentityUser user)
        {
            if (!user.IsModerated)
            {
                return false;
            }

            if (!await SignInManager.CanSignInAsync(user))
            {
                return false;
            }

            //if (await UserManager.IsLockedOutAsync(user))
            //{
            //    return false;
            //}

            return true;
        }
    }
}
