using Essensoft.AspNetCore.Payment.Alipay;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViazyNetCore.Modules.Payment.Alipay.Events;
using ViazyNetCore.Modules.Payment.Events;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.Modules.Internal
{
    public static class WechatPaymentExtensions
    {

        public static IServiceCollection AddWxpayMiddleware(this IServiceCollection services
                , IConfiguration alipayOptions
                , Action<AlipayMiddlewareOptions> configureOptions = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<IAlipayClient, AlipayClient>();
            services.AddSingleton<IAlipayNotifyClient, AlipayNotifyClient>();
            if (alipayOptions != null)
            {
                services.Configure<AlipayOptions>(alipayOptions);
            }
            if (configureOptions == null)
            {
                configureOptions = options => options = new AlipayMiddlewareOptions();
            }
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPaymentHandler, AlipaymentHandler>());
            services.Configure(configureOptions);
            return services;
        }
        public static IServiceCollection AddWxpayMiddleware(this IServiceCollection services
            , Action<AlipayOptions> setupAction
            , Action<AlipayMiddlewareOptions> configureOptions = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<IAlipayClient, AlipayClient>();
            services.AddSingleton<IAlipayNotifyClient, AlipayNotifyClient>();
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
            if (configureOptions == null)
            {
                configureOptions = options => options = new AlipayMiddlewareOptions();
            }
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPaymentHandler, AlipaymentHandler>());
            services.Configure(configureOptions);
            return services;
        }

        public static void UseWxpayMiddleware(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<AlipayNotifyMiddleware>();
        }
    }
}
