using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Server;
using ViazyNetCore.OpenIddict.Domain;

namespace ViazyNetCore.OpenIddict.AspNetCore.ExtensionGrantTypes
{
    public class MyTokenExtensionGrant : ITokenExtensionGrant
    {

        public const string ExtensionGrantName = "MyTokenExtensionGrant";

        public string Name => ExtensionGrantName;
        public async Task<IActionResult> HandleAsync(ExtensionGrantContext context)
        {
            var userToken = context.Request.GetParameter("token").ToString();

            if (string.IsNullOrEmpty(userToken))
            {
                return new ForbidResult(
                    new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest
                    }!));
            }

            var transaction = await context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerFactory>().CreateTransactionAsync();
            transaction.EndpointType = OpenIddictServerEndpointType.Introspection;
            transaction.Request = new OpenIddictRequest
            {
                ClientId = context.Request.ClientId,
                ClientSecret = context.Request.ClientSecret,
                Token = userToken
            };

            var notification = new OpenIddictServerEvents.ProcessAuthenticationContext(transaction);
            var dispatcher = context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerDispatcher>();
            await dispatcher.DispatchAsync(notification);

            if (notification.IsRejected)
            {
                return new ForbidResult(
                    new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                    }));
            }

            var principal = notification.GenericTokenPrincipal;
            if (principal == null)
            {
                return new ForbidResult(
                    new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                    }));
            }

            var userId = principal.FindUserId();
            var userManager = context.HttpContext.RequestServices.GetRequiredService<IdentityUserManager>();
            var user = await userManager.GetByIdAsync(userId.Value);
            var userClaimsPrincipalFactory = context.HttpContext.RequestServices.GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>();
            var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);
            claimsPrincipal.SetScopes(principal.GetScopes());
            claimsPrincipal.SetResources(await GetResourcesAsync(context, principal.GetScopes()));
            await context.HttpContext.RequestServices.GetRequiredService<OpenIddictClaimDestinationsManager>().SetAsync(principal);
            return new Microsoft.AspNetCore.Mvc.SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, claimsPrincipal);
        }

        private async Task<IEnumerable<string>> GetResourcesAsync(ExtensionGrantContext context, ImmutableArray<string> scopes)
        {
            var resources = new List<string>();
            if (!scopes.Any())
            {
                return resources;
            }

            await foreach (var resource in context.HttpContext.RequestServices.GetRequiredService<IOpenIddictScopeManager>().ListResourcesAsync(scopes))
            {
                resources.Add(resource);
            }
            return resources;
        }
    }
}
