using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;

namespace ViazyNetCore.Dapr
{
    public interface IDaprClientFactory
    {
        DaprClient Create(Action<DaprClientBuilder> builderAction = null);

        HttpClient CreateHttpClient(
            string appId = null,
            string daprEndpoint = null,
            string daprApiToken = null
        );
    }
}
