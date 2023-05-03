using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using ViazyNetCore.OpenIddict.Domain;

namespace ViazyNetCore.OpenIddict.DataSeed
{
    public class OpenIddictDataSeed
    {
        private readonly ApplicationManager _applicationManager;

        public OpenIddictDataSeed(ApplicationManager applicationManager)
        {
            this._applicationManager = applicationManager;
        }
        private async Task CreateApplicationAsync(
       [NotNull] string name,
       [NotNull] string type,
       [NotNull] string consentType,
       string displayName,
       string secret,
       List<string> grantTypes,
       List<string> scopes,
       string clientUri = null,
       string redirectUri = null,
       string postLogoutRedirectUri = null,
       List<string> permissions = null)
        {
            if (!string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            {
                throw new ApiException("NoClientSecretCanBeSetForPublicApplications");
            }

            if (string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
            {
                throw new ApiException("TheClientSecretIsRequiredForConfidentialApplications");
            }

            if (!string.IsNullOrEmpty(name) && await _applicationManager.FindByClientIdAsync(name) != null)
            {
                return;
                //throw new BusinessException(L["TheClientIdentifierIsAlreadyTakenByAnotherApplication"]);
            }

            var client = await _applicationManager.FindByClientIdAsync(name);
            if (client == null)
            {
                var application = new ApplicationDescriptor
                {
                    ClientId = name,
                    Type = type,
                    ClientSecret = secret,
                    ConsentType = consentType,
                    DisplayName = displayName,
                    ClientUri = clientUri,
                };

                Check.NotNullOrEmpty(grantTypes, nameof(grantTypes));
                Check.NotNullOrEmpty(scopes, nameof(scopes));

                if (new[] { OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit }.All(grantTypes.Contains))
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);

                    if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
                    }
                }

                if (!redirectUri.IsNullOrWhiteSpace() || !postLogoutRedirectUri.IsNullOrWhiteSpace())
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);
                }

                foreach (var grantType in grantTypes)
                {
                    if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode || grantType == OpenIddictConstants.GrantTypes.Implicit)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                        grantType == OpenIddictConstants.GrantTypes.ClientCredentials ||
                        grantType == OpenIddictConstants.GrantTypes.Password ||
                        grantType == OpenIddictConstants.GrantTypes.RefreshToken ||
                        grantType == OpenIddictConstants.GrantTypes.DeviceCode)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.ClientCredentials)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.Implicit)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.Password)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.RefreshToken)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.DeviceCode)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                        application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device);
                    }

                    if (grantType == OpenIddictConstants.GrantTypes.Implicit)
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
                        if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                        {
                            application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                            application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
                        }
                    }
                }

                var buildInScopes = new[]
                {
                OpenIddictConstants.Permissions.Scopes.Address,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Phone,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles
            };

                foreach (var scope in scopes)
                {
                    if (buildInScopes.Contains(scope))
                    {
                        application.Permissions.Add(scope);
                    }
                    else
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
                    }
                }

                if (redirectUri != null)
                {
                    if (!redirectUri.IsNullOrEmpty())
                    {
                        if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                        {
                            throw new ApiException("InvalidRedirectUri");
                        }

                        if (application.RedirectUris.All(x => x != uri))
                        {
                            application.RedirectUris.Add(uri);
                        }
                    }
                }

                if (postLogoutRedirectUri != null)
                {
                    if (!postLogoutRedirectUri.IsNullOrEmpty())
                    {
                        if (!Uri.TryCreate(postLogoutRedirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                        {
                            throw new ApiException("InvalidPostLogoutRedirectUri");
                        }

                        if (application.PostLogoutRedirectUris.All(x => x != uri))
                        {
                            application.PostLogoutRedirectUris.Add(uri);
                        }
                    }
                }

                if (permissions != null)
                {
                    //await _permissionDataSeeder.SeedAsync(
                    //    ClientPermissionValueProvider.ProviderName,
                    //name,
                    //    permissions,
                    //    null
                    //);
                }

                await _applicationManager.CreateAsync(application);
            }
        }
    }
}
