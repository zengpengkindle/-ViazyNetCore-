using System.Threading.Tasks;

namespace ViazyNetCore.Http.Client;

public static class RemoteServiceConfigurationProviderExtensions
{
    public static Task<RemoteServiceConfiguration> GetConfigurationOrDefaultAsync(
        this IRemoteServiceConfigurationProvider provider)
        => provider.GetConfigurationOrDefaultAsync(RemoteServiceConfigurationDictionary.DefaultName);

    public static Task<RemoteServiceConfiguration> GetConfigurationOrDefaultOrNullAsync(
        this IRemoteServiceConfigurationProvider provider)
        => provider.GetConfigurationOrDefaultOrNullAsync(RemoteServiceConfigurationDictionary.DefaultName);
}