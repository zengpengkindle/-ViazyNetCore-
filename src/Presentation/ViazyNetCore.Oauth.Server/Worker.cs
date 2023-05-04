using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using ViazyNetCore.OpenIddict.AspNetCore.ExtensionGrantTypes;
using ViazyNetCore.OpenIddict.Domain;

namespace Matty.Server;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("device") == null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "device",
                Type = OpenIddictConstants.ClientTypes.Public,
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "Device client",
                Permissions =
                {
                    OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.Endpoints.Device,
                    OpenIddictConstants. Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                }
            });
        }
        if (await manager.FindByClientIdAsync("VaizyApp") == null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "VaizyApp",
                ClientSecret = "1q2w3e*",
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "VaizyNetCore Application",
                PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7284/signout-callback-oidc"),
                    new Uri("https://localhost:7284")
                },
                RedirectUris =
                {
                    new Uri("https://localhost:7284/signin-oidc"),
                    new Uri("https://localhost:7284")
                },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Device,
                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                    OpenIddictConstants.Permissions.Endpoints.Revocation,
                    OpenIddictConstants.Permissions.Endpoints.Logout,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.Implicit,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.Prefixes.GrantType + MyTokenExtensionGrant.ExtensionGrantName,

                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeToken,
                    OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                    OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
                    OpenIddictConstants.Permissions.ResponseTypes.None,
                    OpenIddictConstants.Permissions.ResponseTypes.Token,

                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "VaizyApp"
                }
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
