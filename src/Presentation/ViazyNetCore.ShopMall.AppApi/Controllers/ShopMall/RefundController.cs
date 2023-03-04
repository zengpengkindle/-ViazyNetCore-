using System.Providers;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.ShopMall.AppApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RefundController : ControllerBase
    {
        private readonly RefundService _refundService;
        private readonly ILockProvider _lockProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _memberId => this._httpContextAccessor.HttpContext!.User.GetUserId();
        private readonly string _imgBaseUrl = @"http://localhost:1799";

        public RefundController(RefundService refundService, ILockProvider lockProvider, IHttpContextAccessor httpContextAccessor)
        {
            this._refundService = refundService;
            this._lockProvider = lockProvider;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<RefundTradeModel> FindTradeRefundInfo(string tradeId)
        {
            var result = await this._refundService.FindRefundTradeInfo(this._memberId, tradeId);
            return result;

        }
    }
}
