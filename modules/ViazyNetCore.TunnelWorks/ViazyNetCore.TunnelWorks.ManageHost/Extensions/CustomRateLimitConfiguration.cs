using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.Manage.WebApi
{
    public class CustomRateLimitConfiguration : RateLimitConfiguration
    {

        public override void RegisterResolvers()
        {
            string clientIdHeader = GetClientIdHeader();
            ClientResolvers.Add(new ClientIdResolveContributor(clientIdHeader));
        }

        public CustomRateLimitConfiguration(IOptions<IpRateLimitOptions> ipOptions, IOptions<ClientRateLimitOptions> clientOptions) : base(ipOptions, clientOptions)
        {
        }
    }
    public class ClientIdResolveContributor : IClientResolveContributor
    {
        private readonly string _headerName;

        public ClientIdResolveContributor(string headerName)
        {
            _headerName = headerName;
        }
        public Task<string> ResolveClientAsync(HttpContext httpContext)
        {
            string clientId = httpContext.User.Claims.FirstOrDefault(s => s.Type == _headerName)?.Value;

            return Task.FromResult(clientId);
        }
    }
}
