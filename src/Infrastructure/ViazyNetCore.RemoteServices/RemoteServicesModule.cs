using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Http.Client;

namespace ViazyNetCore.RemoteServices
{
    public class RemoteServicesModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<RemoteServiceOptions>(configuration);
        }
    }
}