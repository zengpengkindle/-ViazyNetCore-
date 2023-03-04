using System.Providers;

namespace ViazyNetCore.Modules.ShopMall
{
    public class TradePayNotifyHandler : IPayNotifyHandler
    {
        private readonly IFreeSql _engine;
        private readonly IEnumerable<IPayTradeHandler> _payTradeHandlers;
        private readonly ILockProvider _lockProvider;

        public TradePayNotifyHandler(IFreeSql engine, IEnumerable<IPayTradeHandler> payTradeHandlers, ILockProvider lockProvider)
        {
            this._engine = engine;
            this._payTradeHandlers = payTradeHandlers;
            this._lockProvider = lockProvider;
        }

        public string TradeType => "ProductTrade";

        public async Task PaySuccessNotify(string outId)
        {
            //var trades = await this._engine.Select<ProductTrade>().Where(p => p.TradeSetId == outId).Select(p => p.Id).ToArrayAsync();
            await FinishPayTradeSetAsync(outId);
        }


        /// <summary>
        /// 支付完成
        /// </summary>
        /// <returns></returns>
        protected async Task FinishPayTradeSetAsync(string outId)
        {
            using (_lockProvider.Lock("Trade" + outId))
            {
                if (outId.IsNull())
                {
                    throw new ApiException("订单不存在");
                }
                var trades = await this._engine.Select<ProductTrade>()
                                  .Where(p => p.PaymentId == outId)
                                  .ToListAsync();

                if (trades == null || trades.Count() == 0)
                    throw new ApiException("订单不存在");

                if (trades.Any(p => p.Status != TradeStatus.UnPay))
                {
                    throw new ApiException("付款订单状态错误。");
                }
                await this._engine.Update<ProductTrade>().Where(p => p.PaymentId == outId).SetDto(
                   new
                   {
                       PayTime = DateTime.Now,
                       Status = TradeStatus.UnDeliver,
                       StatusChangedTime = DateTime.Now
                   }
               ).ExecuteAffrowsAsync();
                foreach (var trade in trades)
                {
                    if (this._payTradeHandlers != null)
                    {
                        foreach (var handler in this._payTradeHandlers)
                        {
                            await handler.OnAfterFinishPayAsync(trade);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 支付完成
        /// </summary>
        /// <returns></returns>
        protected async Task FinishPayTradeAsync(string tradeId)
        {
            var trade = await this._engine.Select<ProductTrade>()
                         .Where(t => t.Id == tradeId)
                         .FirstAsync();

            if (trade == null)
                throw new ApiException("订单不存在");

            if (trade.Status != TradeStatus.UnPay)
                throw new ApiException("付款订单状态异常。");

            if (this._payTradeHandlers != null)
            {
                foreach (var handler in this._payTradeHandlers)
                {
                    await handler.OnAfterFinishPayAsync(trade);
                }
            }
            trade.Status = TradeStatus.UnDeliver;
            trade.StatusChangedTime = DateTime.Now;
            using (var context = this._engine.CreateUnitOfWork())
            {
                await this._engine.Update<ProductTrade>().SetDto(
                   new
                   {
                       trade.Id,
                       trade.Status,
                       trade.StatusChangedTime,
                       PayTime = DateTime.Now
                   }
               ).ExecuteAffrowsAsync();
                context.Commit();
            }
        }

        /// <summary>
        /// 支付关闭
        /// </summary>
        /// <returns></returns>
        protected async Task PayCloseTradeAsync(string tradeId)
        {
            var trade = await this._engine.Select<ProductTrade>()
                           .Where(t => t.Id == tradeId)
                           .FirstAsync();

            if (trade == null)
                throw new ApiException("订单不存在");

            if (trade.Status != TradeStatus.UnPay)
                throw new ApiException("付款订单状态异常。");

            if (this._payTradeHandlers != null)
            {
                foreach (var handler in this._payTradeHandlers)
                {
                    await handler.OnAfterClosePayAsync(trade);
                }
            }
            trade.Status = TradeStatus.Close;
            trade.StatusChangedTime = DateTime.Now;
            await this._engine.Update<ProductTrade>().SetDto(
                 new
                 {
                     trade.Id,
                     trade.Status,
                     trade.StatusChangedTime
                 }
             ).ExecuteAffrowsAsync();
        }
    }
}
