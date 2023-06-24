using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.Dapr
{
    [Injection]
    public class DaprClientFactory : IDaprClientFactory
    {
        protected DaprOptions DaprOptions { get; }
        protected JsonSerializerOptions JsonSerializerOptions { get; }
        protected IDaprApiTokenProvider DaprApiTokenProvider { get; }

        public DaprClientFactory(
            IOptions<DaprOptions> options,
            IOptions<SystemTextJsonSerializerOptions> systemTextJsonSerializerOptions,
            IDaprApiTokenProvider daprApiTokenProvider)
        {
            DaprApiTokenProvider = daprApiTokenProvider;
            DaprOptions = options.Value;
            JsonSerializerOptions = CreateJsonSerializerOptions(systemTextJsonSerializerOptions.Value);
        }

        public virtual DaprClient Create(Action<DaprClientBuilder> builderAction = null)
        {
            var builder = new DaprClientBuilder()
                .UseJsonSerializationOptions(JsonSerializerOptions);

            if (!DaprOptions.HttpEndpoint.IsNullOrWhiteSpace())
            {
                builder.UseHttpEndpoint(DaprOptions.HttpEndpoint);
            }

            if (!DaprOptions.GrpcEndpoint.IsNullOrWhiteSpace())
            {
                builder.UseGrpcEndpoint(DaprOptions.GrpcEndpoint);
            }

            var apiToken = DaprApiTokenProvider.GetDaprApiToken();
            if (!apiToken.IsNullOrWhiteSpace())
            {
                builder.UseDaprApiToken(apiToken);
            }

            builderAction?.Invoke(builder);

            return builder.Build();
        }

        public virtual HttpClient CreateHttpClient(
            string appId = null,
            string daprEndpoint = null,
            string daprApiToken = null)
        {
            if (daprEndpoint.IsNullOrWhiteSpace() &&
               !DaprOptions.HttpEndpoint.IsNullOrWhiteSpace())
            {
                daprEndpoint = DaprOptions.HttpEndpoint;
            }

            return DaprClient.CreateInvokeHttpClient(
                appId,
                daprEndpoint,
                daprApiToken ?? DaprApiTokenProvider.GetDaprApiToken()
            );
        }

        protected virtual JsonSerializerOptions CreateJsonSerializerOptions(SystemTextJsonSerializerOptions systemTextJsonSerializerOptions)
        {
            return new JsonSerializerOptions(systemTextJsonSerializerOptions.JsonSerializerOptions);
        }
    }
}
