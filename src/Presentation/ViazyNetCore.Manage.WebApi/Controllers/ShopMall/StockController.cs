using System.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.Manage.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Area("shopmall")]
    [Permission(PermissionIds.Stock)]
    public class StockController : ControllerBase
    {
        private readonly StockService _stockService;
        private readonly ILockProvider _lockProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private long _memberId => this._httpContextAccessor.HttpContext!.User.GetUserId();


        public StockController(StockService stockService, ILockProvider lockProvider, IHttpContextAccessor httpContextAccessor)
        {
            this._stockService = stockService;
            this._lockProvider = lockProvider;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<StockLogModel> FindStockLogs(StockPageArgments args)
        {
            var result = await this._stockService.GetStockUpdateLog(args);
            return result;
        }

        [HttpPost]
        public async Task UpdateStock(string stockId, int inStockNum, string remark)
        {
            await this._stockService.UpdateInStock(stockId, inStockNum, remark, this._memberId);
        }
    }
}
