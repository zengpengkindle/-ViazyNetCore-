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
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _memberId => this._httpContextAccessor.HttpContext!.User.GetUserId();
        private readonly string _imgBaseUrl = @"http://localhost:1799";

        public TradeController(TradeService tradeService, ILockProvider lockProvider, IHttpContextAccessor httpContextAccessor)
        {
            this._tradeService = tradeService;
            this._lockProvider = lockProvider;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<CreateTradeSetModel> BeforeTrade(List<BeforeTradeItem> items)
        {
            var result = await this._tradeService.BeforeCreateTradeAsync(_memberId, items);
            for (int i = 0; i < result.ShopTrades.Count; i++)
            {
                for (int j = 0; j < result.ShopTrades[i].Items.Count; j++)
                {
                    result.ShopTrades[i].Items[j].ImgUrl = result.ShopTrades[i].Items[j].ImgUrl.Replace("/upload/", _imgBaseUrl + "/upload/");
                }
            }
            return result;
        }

        [HttpPost]
        public async Task<string[]> CreateTrade(CreateTradeSetModel createTradeSet)
        {
            var result = await this._tradeService.CreateTradeSetAsync(_memberId, createTradeSet);
            return result;
        }

        [HttpPost]
        public async Task<PageData<TradeDetailModel>> FindMyTrades(TradePageArgments args)
        {
            args.MemberId = _memberId;
            var result = await this._tradeService.FindTradeList(args);
            foreach (var item in result.Rows)
            {
                foreach (var order in item.Orders)
                {
                    order.ImgUrl = order.ImgUrl.Replace("/upload/", _imgBaseUrl + "/upload/");
                }
            }
            return result;
        }

        [HttpPost]
        public async Task<TradeDetailModel> FindTrade(string tradeId)
        {
            var result = await this._tradeService.GetTradeDetail(_memberId, tradeId);
            return result;
        }

        [HttpPost]
        public async Task<TradeSetModel> FindTradesPayInfo(string[] tradeIds)
        {
            var result = await this._tradeService.GetTradesPayInfo(tradeIds, _memberId);
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
