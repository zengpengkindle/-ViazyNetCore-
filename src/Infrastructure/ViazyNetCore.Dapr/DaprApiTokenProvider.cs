using Microsoft.Extensions.Options;

namespace ViazyNetCore.Dapr
{
    [Injection]
    public class DaprApiTokenProvider : IDaprApiTokenProvider
    {
        protected DaprOptions Options { get; }

        public DaprApiTokenProvider(IOptions<DaprOptions> options)
        {
            Options = options.Value;
        }

        public virtual string GetDaprApiToken()
        {
            return Options.DaprApiToken;
        }

        public virtual string GetAppApiToken()
        {
            return Options.AppApiToken;
        }
    }

}