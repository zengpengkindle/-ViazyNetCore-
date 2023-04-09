using System.Providers;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.ShopMall.AppApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RefundController : BaseController
    {
        private readonly RefundService _refundService;
        private readonly ILockProvider _lockProvider;

        public RefundController(RefundService refundService, ILockProvider lockProvider)
        {
            this._refundService = refundService;
            this._lockProvider = lockProvider;
        }

        [HttpPost]
        public async Task<RefundTradeModel> FindTradeRefundInfo(string tradeId)
        {
            var result = await this._refundService.FindRefundTradeInfo(this.MemberId, tradeId);
            return result;
        }
    }
}
