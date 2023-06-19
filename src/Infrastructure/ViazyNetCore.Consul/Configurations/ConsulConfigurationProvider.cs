using Microsoft.Extensions.Configuration;
using ViazyNetCore.Gateway.Configurations;

namespace ViazyNetCore.Consul.Configurations
{
    public class ConsulConfigurationProvider : FileConfigurationProvider
    {
        public ConsulConfigurationProvider(ConsulConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            var parser = new JsonConfigurationParser();
            this.Data = parser.Parse(stream, null);
        }
    }
}
