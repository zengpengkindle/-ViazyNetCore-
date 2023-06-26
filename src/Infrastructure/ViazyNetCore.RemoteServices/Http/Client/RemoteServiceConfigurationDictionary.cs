using System.Collections.Generic;

namespace ViazyNetCore.Http.Client;

public class RemoteServiceConfigurationDictionary : Dictionary<string, RemoteServiceConfiguration>
{
    public const string DefaultName = "Default";

    public RemoteServiceConfiguration Default
    {
        get => this.GetOrDefault(DefaultName);
        set => this[DefaultName] = value;
    }

    public RemoteServiceConfiguration GetConfigurationOrDefault(string name)
    {
        return this.GetOrDefault(name)
               ?? Default
               ?? throw new Exception($"Remote service '{name}' was not found and there is no default configuration.");
    }

    public RemoteServiceConfiguration GetConfigurationOrDefaultOrNull(string name)
    {
        return this.GetOrDefault(name)
               ?? Default;
    }
}
