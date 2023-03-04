﻿using Aoite.WebApi;
using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.Alipay.Notify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    public class AlipayNotifyMiddleware
    {
        private RequestDelegate _nextDelegate;
        private readonly PaymentService _paymentService;
        private readonly IAlipayNotifyClient _notifyClient;
        private readonly AlipayMiddlewareOptions _options;

        public AlipayNotifyMiddleware(RequestDelegate nextDelegate, PaymentService paymentService, IAlipayNotifyClient notifyClient, IOptions<AlipayMiddlewareOptions> options)
        {
            _nextDelegate = nextDelegate;
            this._paymentService = paymentService;
            this._notifyClient = notifyClient;
            this._options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments("/"+_options.NotifyUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                var methods = httpContext.Request.Path.Value.RemoveStarts(14);
                var appRoute = _options.AppNotifyRoute;
                var pageRoute = _options.PageNotifyRoute;
                var wapRoute = _options.WapNotifyRoute;
                if(methods.StartsWith("/"))
                    methods = methods.RemoveStarts(1);
                if (methods.iEquals(appRoute))
                {
                    var notify = await _notifyClient.ExecuteAsync<AlipayTradePrecreateNotify>(httpContext.Request);
                    if ("TRADE_SUCCESS" == notify.TradeStatus)
                    {
                        await _paymentService.PaySuccessNotify(notify.OutTradeNo);
                        await AlipayNotifyResult.Success.ExecuteResultAsync(httpContext);
                        return;
                    }
                }
                else if (methods.iEquals(pageRoute))
                {
                    var notify = await _notifyClient.ExecuteAsync<AlipayTradePagePayNotify>(httpContext.Request);
                    if ("TRADE_SUCCESS" == notify.TradeStatus)
                    {
                        await _paymentService.PaySuccessNotify(notify.OutTradeNo);
                        await AlipayNotifyResult.Success.ExecuteResultAsync(httpContext);
                        return;
                    }
                }
                else if (methods.iEquals(wapRoute))
                {
                    var notify = await _notifyClient.ExecuteAsync<AlipayTradeWapPayNotify>(httpContext.Request);
                    if ("TRADE_SUCCESS" == notify.TradeStatus)
                    {
                        await _paymentService.PaySuccessNotify(notify.OutTradeNo);
                        await AlipayNotifyResult.Success.ExecuteResultAsync(httpContext);
                        return;
                    }
                }
                await new OkResult().ExecuteResultAsync(httpContext);
            }
            else
            {
                await _nextDelegate.Invoke(httpContext);
            }
        }
    }
}
