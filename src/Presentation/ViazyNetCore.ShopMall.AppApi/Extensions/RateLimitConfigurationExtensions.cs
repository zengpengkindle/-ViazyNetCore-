using System.IdentityModel.Tokens.Jwt;
using AspNetCoreRateLimit;

namespace ViazyNetCore.ShopMall.AppApi.Extensions
{
    public static class RateLimitConfigurationExtensions
    {
        public static void AddIpRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            #region IP限流

            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

            //内存
            services.AddMemoryCache();
            services.AddInMemoryRateLimiting();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            #endregion IP限流
        }

        public static void AddRequestRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddQueuePolicy(options =>
            {
                options.MaxConcurrentRequests = 500;
                options.RequestQueueLimit = 2000;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<ClientRateLimitOptions>(configuration.GetSection("ClientRateLimiting"));
            services.Configure<ClientRateLimitPolicies>(configuration.GetSection("ClientRateLimiting:ClientRateLimitPolicies"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, CustomRateLimitConfiguration>();
        }

        public static async void UseRequestRateLimiting(this WebApplication app)
        {
            var clientPolicy = app.Services.GetRequiredService<IClientPolicyStore>();
            await clientPolicy.SeedAsync();
            //登录后开启限流，未登录不限流
            app.UseWhen(x => x.User?.Identity?.IsAuthenticated ?? false, builder =>
            {
                app.UseClientRateLimiting();
            });

            app.UseConcurrencyLimiter();

        }
    }
}
