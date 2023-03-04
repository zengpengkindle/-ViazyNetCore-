using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.Alipay.Domain;
using Essensoft.AspNetCore.Payment.Alipay.Request;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    public class AlipaymentHandler : IPaymentHandler
    {
        private readonly IAlipayClient _client;
        private readonly IAlipayNotifyClient _notifyClient;
        private readonly AlipayMiddlewareOptions _alipayOptions;

        public Buyway Buyway => Buyway.AliPay;

        public AlipaymentHandler(IAlipayClient client, IAlipayNotifyClient notifyClient, IOptions<AlipayMiddlewareOptions> alipayOptions)
        {
            _client = client;
            _notifyClient = notifyClient;
            this._alipayOptions = alipayOptions.Value;
        }

        public async Task<PayCreateValue> CreatePayment(PayMediaType payMediaType, TradeViewModel tradeModel)
        {
            switch (payMediaType)
            {
                case PayMediaType.PC:
                    return await PagePay(tradeModel);
                case PayMediaType.WAP:
                    return await WapPay(tradeModel);
                case PayMediaType.APP:
                    return await AppPay(tradeModel);
                case PayMediaType.WeiXinMP:
                default:
                    break;
            };
            throw new NotImplementedException();
        }


        public async Task<PayCreateValue> PagePay(TradeViewModel viewModel)
        {
            var model = new AlipayTradePagePayModel
            {
                Body = viewModel.Body,
                Subject = viewModel.Subject,
                TotalAmount = viewModel.TotalAmount.ToString("0.00"),
                OutTradeNo = viewModel.OutTradeNo,
                ProductCode = "FAST_INSTANT_TRADE_PAY"
            };
            var req = new AlipayTradePagePayRequest();
            req.SetBizModel(model);

            var notifyUrl = GA.UrlCombine(this._alipayOptions.Host,this._alipayOptions.NotifyUrl, this._alipayOptions.PageNotifyRoute);

            req.SetNotifyUrl(notifyUrl);
            req.SetReturnUrl(viewModel.ReturnUrl);

            var response = await _client.PageExecuteAsync(req);

            var payCreateValue = new PayCreateValue()
            {
                BodyOrderId = viewModel.OutTradeNo,
                Content = response.Body,
                ContentType = PayContentType.Html
            };
            return payCreateValue;
        }
        public async Task<PayCreateValue> WapPay(TradeViewModel viewModel)
        {
            var model = new AlipayTradeWapPayModel
            {
                Body = viewModel.Body,
                Subject = viewModel.Subject,
                TotalAmount = viewModel.TotalAmount.ToString("0.00"),
                OutTradeNo = viewModel.OutTradeNo,
                ProductCode = "QUICK_WAP_WAY"
            };
            var req = new AlipayTradeWapPayRequest();
            req.SetBizModel(model);

            var notifyUrl = GA.UrlCombine(this._alipayOptions.Host, this._alipayOptions.NotifyUrl, this._alipayOptions.WapNotifyRoute);

            req.SetNotifyUrl(notifyUrl);
            req.SetReturnUrl(viewModel.ReturnUrl);

            var response = await _client.PageExecuteAsync(req);

            var payCreateValue = new PayCreateValue()
            {
                BodyOrderId = viewModel.OutTradeNo,
                Content = response.Body,
                ContentType = PayContentType.Html
            };
            return payCreateValue;
        }

        public async Task<PayCreateValue> AppPay(TradeViewModel viewModel)
        {
            var model = new AlipayTradeAppPayModel
            {
                OutTradeNo = viewModel.OutTradeNo,
                Subject = viewModel.Subject,
                TotalAmount = viewModel.TotalAmount.ToString("0.00"),
                Body = viewModel.Body,
                ProductCode = "QUICK_MSECURITY_PAY"
            };
            var req = new AlipayTradeAppPayRequest();

            var notifyUrl = GA.UrlCombine(this._alipayOptions.Host, this._alipayOptions.NotifyUrl, this._alipayOptions.AppNotifyRoute);

            req.SetBizModel(model);
            req.SetNotifyUrl(notifyUrl);

            var response = await _client.SdkExecuteAsync(req);

            var payCreateValue = new PayCreateValue()
            {
                BodyOrderId = viewModel.OutTradeNo,
                Content = response.Body,
                ContentType = PayContentType.Scripts
            };
            return payCreateValue;
        }

    }

}
