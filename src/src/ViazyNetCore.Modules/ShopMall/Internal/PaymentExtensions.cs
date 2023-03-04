﻿using Essensoft.AspNetCore.Payment.Alipay;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace ViazyNetCore.Modules.ShopMall
{
    public static class PaymentExtensions
    {

        public static IServiceCollection AddAlipayMiddleware(this IServiceCollection services
                , IConfiguration alipayOptions
                , Action<AlipayMiddlewareOptions> configureOptions = null)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<IAlipayClient, AlipayClient>();
            services.AddSingleton<IAlipayNotifyClient, AlipayNotifyClient>();
            if(alipayOptions != null)
            {
                services.Configure<AlipayOptions>(alipayOptions);
            }
            if(configureOptions == null)
            {
                configureOptions = options => options = new AlipayMiddlewareOptions();
            }
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPaymentHandler, AlipaymentHandler>());
            services.Configure(configureOptions);
            return services;
        }
        public static IServiceCollection AddAlipayMiddleware(this IServiceCollection services
            , Action<AlipayOptions> setupAction
            , Action<AlipayMiddlewareOptions> configureOptions = null)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<IAlipayClient, AlipayClient>();
            services.AddSingleton<IAlipayNotifyClient, AlipayNotifyClient>();
            if(setupAction != null)
            {
                services.Configure(setupAction);
            }
            if(configureOptions == null)
            {
                configureOptions = options => options = new AlipayMiddlewareOptions();
            }
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPaymentHandler, AlipaymentHandler>());
            services.Configure(configureOptions);
            return services;
        }

        public static void UseAlipayMiddleware(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<AlipayNotifyMiddleware>();
        }
    }
}
