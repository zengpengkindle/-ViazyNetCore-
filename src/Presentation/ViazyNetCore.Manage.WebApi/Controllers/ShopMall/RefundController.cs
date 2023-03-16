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
    [Permission(PermissionIds.Refund)]
    public class RefundController : ControllerBase
    {
        private readonly RefundService _refundService;
        private readonly ILockProvider _lockProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _memberId => this._httpContextAccessor.HttpContext!.User.GetUserId();

        public RefundController(RefundService refundService, ILockProvider lockProvider, IHttpContextAccessor httpContextAccessor)
        {
            this._refundService = refundService;
            this._lockProvider = lockProvider;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<PageData<RefundListModel>> FindAll(RefundArgments args)
        {
            return await this._refundService.FindRefundTrades(args);
        }

        //public async Task<RefundStepModel> FindRefund(string memberId, string refundId)
        //{
        //    var refundStepModel = new RefundStepModel
        //    {
        //        Refund = await this._refundService.FindRefundTradeDetail(memberId, refundId, RefundTradeLogType.Seller),
        //        Steps = await this._refundService.FindRefundStepLogs(memberId, refundId)
        //    };
        //    return refundStepModel;
        //}

        public class RefundStepModel
        {
            public RefundDetailModel Refund { get; set; }
            public List<RefundStepLog> Steps { get; set; }
        }

        [HttpPost]
        public async Task Submit(RefundParamsModel paramsModel)
        {
            //1.验证步骤合法
            //_refundService内有验证
            //2.填写部分值
            paramsModel.UserId = this._memberId;
            paramsModel.HandleUserType = RefundTradeLogType.Seller;
            //3.执行服务方法
            await this._refundService.HandleRefund(paramsModel);

            //4.返回结果
            //throw new NotImplementedException();
        }
    }
}
