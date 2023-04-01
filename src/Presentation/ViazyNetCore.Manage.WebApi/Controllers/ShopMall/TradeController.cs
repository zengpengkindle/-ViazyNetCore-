using System.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.Manage.WebApi.Controllers
{
    [Area("shopmall")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Permission(PermissionIds.Trade)]
    public class TradeController : ControllerBase
    {
        private readonly TradeService _tradeService;
        private readonly LogisticsService _logisticsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILockProvider _lockProvider;

        private string _memberId => this._httpContextAccessor.HttpContext!.User.GetUserId();

        public TradeController(TradeService tradeService, ILockProvider lockProvider, LogisticsService logisticsService, IHttpContextAccessor httpContextAccessor)
        {
            this._tradeService = tradeService;
            this._lockProvider = lockProvider;
            this._logisticsService = logisticsService;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<PageData<TradeDetailModel>> FindAll(TradePageArgments args)
        {
            return await this._tradeService.FindTradeList(args);
        }

        [HttpPost]
        public async Task<TradeDetailModel> FindTrade(string tradeId)
        {
            return await this._tradeService.GetTradeDetail(null, tradeId);
        }

        [HttpPost]
        public async Task<List<SimpleLogisticsCompanyRes>> FindWlList()
        {
            var table = await this._logisticsService.GetLogisticsCompanys();

            var query = from r in table
                        orderby r.Name
                        select new SimpleLogisticsCompanyRes
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Code = r.Code
                        };
            return query.ToList();
        }

        [HttpPost]
        public async Task<DeliverFailRes> DeliverTrades(BatchDeliveryRes model)
        {
            int Fail = 0;
            var FailIds = new List<string>();
            for (int i = 0; i < model.TradeIds.Length; i++)
            {
                if (i >= 1)
                {
                    model.Delivery.LogisticsFee = 0;
                }
                try
                {
                    await this._tradeService.TradeDeliveryAsync(model.TradeIds[i], model.Delivery);
                }
                catch (ApiException ex)
                {
                    Fail++;
                    FailIds.Add($"订单{model.TradeIds[i]}:" + ex.Message);
                }
            }
            return new DeliverFailRes { Fail = Fail, FailIds = FailIds };
        }

        [HttpPost]
        public async Task ModifyTradeAddress(TradeAddressReq model)
        {
            await this._tradeService.TradeAddressChangeAsync(model.Id, model.ReceiverName, model.ReceiverMobile, model.ReceiverProvince, model.ReceiverCity, model.ReceiverDistrict, model.ReceiverDetail);
        }

        [HttpPost]
        public async Task ChangeTradeDeliver(string tradeId, DeliveryModel deliveryModel)
        {
            await this._tradeService.TradeDeliveryChangeAsync(tradeId, deliveryModel);
        }
    }
}
