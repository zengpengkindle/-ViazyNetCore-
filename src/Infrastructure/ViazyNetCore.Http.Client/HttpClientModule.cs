using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Http.Client.DynamicProxying;

namespace ViazyNetCore.Http.Client
{
    public class HttpClientModule:InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient();
            context.Services.AddTransient(typeof(DynamicHttpProxyInterceptorClientProxy<>));
        }
    }
}