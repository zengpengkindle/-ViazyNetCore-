using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViazyNetCore.Modules.ShopMall
{
    public static class ShopExtensions
    {
        public static IServiceCollection AddShop(this IServiceCollection services, IMemberService memberService = null)
        {
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IPayNotifyHandler, TradePayNotifyHandler>());
            services.AddScoped<PaymentService>();
            if (memberService == null)
                services.AddScoped<IMemberService, DefaultMemberService>();

            services.AddScoped<ProductService>()
                .AddScoped<LogisticsService>()
                .AddScoped<CartService>()
                .AddScoped<AddressService>()
                .AddScoped<TradeService>()
                .AddScoped<RefundService>()
                .AddScoped<ProductService>()
                .AddScoped<StockService>()
                .AddScoped<RefundService>();
            return services;
        }
    }
}
