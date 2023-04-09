using System;
using System.Providers;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Model;

namespace ViazyNetCore.ShopMall.AppApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TradeController : BaseController
    {
        private readonly TradeService _tradeService;
        private readonly ILockProvider _lockProvider;

        public TradeController(TradeService tradeService, ILockProvider lockProvider)
        {
            this._tradeService = tradeService;
            this._lockProvider = lockProvider;
        }

        [HttpPost]
        public async Task<CreateTradeSetModel> BeforeTrade(List<BeforeTradeItem> items)
        {
            var result = await this._tradeService.BeforeCreateTradeAsync(this.MemberId, items);
            foreach (var trade in result.ShopTrades)
            {
                foreach (var item in trade.Items)
                {
                    item.ImgUrl = item.ImgUrl.ToCdnUrl();
                }
            }
            result.Num = result.ShopTrades.Sum(p => p.Items.Sum(p => p.Num));
            return result;
        }

        [HttpPost]
        public async Task<string[]> BindTrade(CreateTradeSetModel createTradeSet)
        {
            var result = await this._tradeService.CreateTradeSetAsync(this.MemberId, createTradeSet);
            return result;
        }

        [HttpPost]
        public async Task<string[]> CreateTrade(List<BeforeTradeItem> items)
        {
            var createTradeSet = await this._tradeService.BeforeCreateTradeAsync(this.MemberId, items);
            var result = await this._tradeService.CreateTradeSetAsync(this.MemberId, createTradeSet);
            return result;
        }

        [HttpPost]
        public async Task<PageData<TradeDetailModel>> FindMyTrades(TradePageReq request)
        {
            var args = new TradePageArgments()
            {
                TradeStatus = request.TradeStatus,
                MemberId = MemberId
            };

            var result = await this._tradeService.FindTradeList(args);
            foreach (var item in result.Rows)
            {
                foreach (var order in item.Orders)
                {
                    order.ImgUrl = order.ImgUrl.ToCdnUrl();
                }
            }
            return result;
        }

        [HttpPost]
        public async Task<TradeDetailModel> FindTrade(string tradeId)
        {
            var result = await this._tradeService.GetTradeDetail(this.MemberId, tradeId);
            return result;
        }

        [HttpPost]
        public async Task<TradeSetModel> FindTradesPayInfo(string[] tradeIds)
        {
            var result = await this._tradeService.GetTradesPayInfo(tradeIds, this.MemberId);
            return result;
        }

        [HttpPost]
        public async Task<PayCreateValue> CreateTradeAliPay(string[] tradeIds)
        {
            var result = await this._tradeService.CreatePayTradeSetAsync(tradeIds, Buyway.AliPay, PayMediaType.WAP, "http://wlhsa.ngrok.xiaomiqiu.cn/app/shop__trade__list.html");
            return result;
        }
    }
}
