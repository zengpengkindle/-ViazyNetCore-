using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class PaymentService
    {
        private readonly IFreeSql _engine;
        private readonly ILogger<PaymentService> _logger;
        private readonly IEnumerable<IPaymentHandler> _paymentHandlers;
        private readonly IEnumerable<IPayNotifyHandler> _payNotifyHandlers;

        public PaymentService(IFreeSql engine, ILogger<PaymentService> logger, IEnumerable<IPaymentHandler> paymentHandlers, IEnumerable<IPayNotifyHandler> payNotifyHandlers)
        {
            this._engine = engine;
            this._logger = logger;
            this._paymentHandlers = paymentHandlers;
            this._payNotifyHandlers = payNotifyHandlers;
        }

        public async Task<PayCreateValue> CreatePrePayment(Buyway buyway, PayMediaType payMediaType, TradeViewModel tradeModel)
        {
            if (tradeModel.OutTradeNo.IsNull())
                tradeModel.OutTradeNo = Snowflake<MemberPayment>.NextIdString();

            var memberPayment = new MemberPayment
            {
                Id = tradeModel.OutTradeNo,
                Amount = tradeModel.TotalAmount,
                CreateTime = DateTime.Now,
                MemberId = tradeModel.MemberId,
                PayChannel = payMediaType.ToString(),
                PayType = buyway,
                Status = ApplyStatus.Apply,
                StatusChangeTime = DateTime.Now,
                TradeType = "ProductTrade"
            };

            await this._engine.Insert<MemberPayment>().AppendData(memberPayment).ExecuteAffrowsAsync();
            if (this._paymentHandlers != null)
            {
                var handler = this._paymentHandlers.Where(p => p.Buyway == buyway).FirstOrDefault();
                if (handler != null)
                {
                    var prePayResult = await handler.CreatePayment(payMediaType, tradeModel);
                    return prePayResult;
                }
            }
            throw new NotImplementedException("不支持该类型支付");
        }

        public async Task PaySuccessNotify(string outId)
        {
            var payment = await this._engine.Select<MemberPayment>().Where(p => p.Id == outId).FirstAsync();
            if (payment == null)
            {
                return;
            }
            if (this._payNotifyHandlers != null)
            {
                foreach (var handler in _payNotifyHandlers.Where(p => p.TradeType.iEquals(payment.TradeType)))
                {
                    await handler.PaySuccessNotify(outId);
                }
            }
            payment.Status = ApplyStatus.Success;
            await this._engine.Update<MemberPayment>().SetDto(new
            {
                payment.Id,
                Status = ApplyStatus.Success,
                StatusChangeTime = DateTime.Now
            }).ExecuteAffrowsAsync();
        }
    }
}
