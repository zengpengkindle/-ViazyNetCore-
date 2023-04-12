using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Essensoft.Paylink.WeChatPay;
using Essensoft.Paylink.WeChatPay.V2;
using Essensoft.Paylink.WeChatPay.V2.Notify;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ViazyNetCore.ShopMall.AppApi.Controllers
{
    /// <summary>
    /// 微信支付异步通知
    /// </summary>
    [Area("shopmall")]
    [Route("Notify/[controller]/[action]")]
    public class WeChatPayController : ControllerBase
    {
        private readonly IOptions<WeChatPayOptions> _optionsAccessor;
        private readonly ILogger<WeChatPayController> _logger;
        private readonly IWeChatPayNotifyClient _client;

        public WeChatPayController(IOptions<WeChatPayOptions> optionsAccessor, ILogger<WeChatPayController> logger, IWeChatPayNotifyClient client)
        {
            this._optionsAccessor = optionsAccessor;
            this._logger = logger;
            this._client = client;
        }

        /// <summary>
        /// 统一下单支付结果通知
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Unifiedorder()
        {
            try
            {
                var notify = await this._client.ExecuteAsync<WeChatPayUnifiedOrderNotify>(this.Request, this._optionsAccessor.Value);
                if (notify.ReturnCode == WeChatPayCode.Success)
                {
                    // await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.WeChatPayNotice, JsonConvert.SerializeObject(notify));
                    return WeChatPayNotifyResult.Success;
                }
                this._logger.LogInformation("微信支付成功回调", JsonConvert.SerializeObject(notify));
                return NoContent();
            }
            catch (Exception ex)
            {
                this._logger.LogError("微信支付回调异常", ex);
                return NoContent();
            }
        }

        /// <summary>
        /// 退款结果通知
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Refund()
        {
            try
            {
                var notify = await _client.ExecuteAsync<WeChatPayRefundNotify>(Request, _optionsAccessor.Value);
                this._logger.LogInformation("微信退款结果通知", JsonConvert.SerializeObject(notify));

                if (notify.ReturnCode == WeChatPayCode.Success)
                    if (notify.RefundStatus == WeChatPayCode.Success)
                    {
                        //Console.WriteLine("OutTradeNo: " + notify.OutTradeNo);
                        var memo = JsonConvert.SerializeObject(notify);
                        //await _billRefundServices.UpdateAsync(p => new CoreCmsBillRefund { memo = memo }, p => p.refundId == notify.OutTradeNo);
                        return WeChatPayNotifyResult.Success;
                    }
                return NoContent();
            }
            catch (Exception ex)
            {
                this._logger.LogError("微信退款结果通知异常", ex);
                return NoContent();
            }
        }
    }
}
