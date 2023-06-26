using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.Http.Client;
[Injection(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
public class RemoteServiceConfigurationProvider : IRemoteServiceConfigurationProvider
{
    protected RemoteServiceOptions Options { get; }

    public RemoteServiceConfigurationProvider(IOptionsMonitor<RemoteServiceOptions> options)
    {
        Options = options.CurrentValue;
    }

    public Task<RemoteServiceConfiguration> GetConfigurationOrDefaultAsync(string name)
    {
        return Task.FromResult(Options.RemoteServices.GetConfigurationOrDefault(name));
    }

    public Task<RemoteServiceConfiguration> GetConfigurationOrDefaultOrNullAsync(string name)
    {
        return Task.FromResult(Options.RemoteServices.GetConfigurationOrDefaultOrNull(name));
    }
}
