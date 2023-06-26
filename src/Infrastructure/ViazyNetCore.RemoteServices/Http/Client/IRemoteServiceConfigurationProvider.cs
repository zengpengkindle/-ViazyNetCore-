using System.Threading.Tasks;

namespace ViazyNetCore.Http.Client;

public interface IRemoteServiceConfigurationProvider
{
    Task<RemoteServiceConfiguration> GetConfigurationOrDefaultAsync(string name);

    Task<RemoteServiceConfiguration> GetConfigurationOrDefaultOrNullAsync(string name);
}
