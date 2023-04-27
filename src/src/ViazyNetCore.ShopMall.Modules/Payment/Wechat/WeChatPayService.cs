using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ViazyNetCore.Configuration;
using ViazyNetCore.Model.Payment;
using Essensoft.Paylink.WeChatPay.V2;
using ViazyNetCore.Modules.ShopMall;
using Microsoft.Extensions.Options;
using Essensoft.Paylink.WeChatPay;
using Essensoft.Paylink.WeChatPay.V2.Request;
using ViazyNetCore.Modules.Internal;

namespace ViazyNetCore.Modules.Payment
{
    public class WeChatPayService
    {
        private readonly IWeChatPayClient _client;
        private readonly IMemberService _memberService;
        private readonly IWechatAuthService _wechatAuthService;
        private readonly IOptions<WeChatPayOptions> _optionsAccessor;

        public WeChatPayService(IWeChatPayClient client
            , IMemberService memberService
            , IWechatAuthService wechatAuthService
            , IOptions<WeChatPayOptions> optionsAccessor
            )
        {
            this._client = client;
            this._memberService = memberService;
            this._wechatAuthService = wechatAuthService;
            this._optionsAccessor = optionsAccessor;
        }

        /// <summary>
        /// 发起支付
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<object> CreatePay(BillPayments entity, long userId)
        {
            var weChatPayUrl = AppSettingsConstVars.PayCallBackWeChatPayUrl;
            if (string.IsNullOrEmpty(weChatPayUrl))
            {
                throw new ApiException("未获取到配置的通知地址");
            }

            var tradeType = WeiChatPayTradeType.JSAPI.ToString();
            if (!string.IsNullOrEmpty(entity.Parameters))
            {
                var jobj = (JObject)JsonConvert.DeserializeObject(entity.Parameters);
                if (jobj != null && jobj.ContainsKey("trade_type"))
                    tradeType = GetTradeType(jobj["trade_type"].ToString());
            }

            var openId = string.Empty;
            if (tradeType == WeiChatPayTradeType.JSAPI.ToString())
            {
                var userAccount = await _memberService.GetMemberNameByMemberId(userId);
                if (userAccount == null)
                {
                    throw new ApiException("用户账户获取失败");
                }

                //if (userAccount.userWx <= 0)
                //{
                //    throw new ApiException("账户关联微信用户信息获取失败");
                //}
                var userWechatInfo = await _wechatAuthService.GetWechatInfoByMemberId(userId);
                //var user = await _userWeChatInfoServices.QueryByClauseAsync(p => p.id == userAccount.userWx);
                if (userWechatInfo == null)
                {
                    throw new ApiException("账户关联微信用户信息获取失败");
                }

                openId = userWechatInfo.OpenId;
            }

            var request = new WeChatPayUnifiedOrderRequest
            {
                Body = entity.PayTitle.Length > 50 ? entity.PayTitle[..50] : entity.PayTitle,
                OutTradeNo = entity.PaymentId,
                TotalFee = Convert.ToInt32(entity.Money * 100),
                SpBillCreateIp = entity.Ip,
                NotifyUrl = weChatPayUrl,
                TradeType = tradeType,
                OpenId = openId
            };

            var response = await _client.ExecuteAsync(request, _optionsAccessor.Value);
            if (response.ReturnCode == WeChatPayCode.Success && response.ResultCode == WeChatPayCode.Success)
            {
                var req = new WeChatPayJsApiSdkRequest
                {
                    Package = "prepay_id=" + response.PrepayId
                };

                var parameter = await _client.ExecuteAsync(req, _optionsAccessor.Value);
                // 将参数(parameter)给 公众号前端 让他在微信内H5调起支付(https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7&index=6)
                parameter.Add("paymentId", entity.PaymentId);

                return response;
            }
            else
            {
                throw new ApiException("微信建立支付请求失败");
            }
        }

        /// <summary>
        /// 用户退款
        /// </summary>
        /// <param name="refundInfo">退款单数据</param>
        /// <param name="paymentInfo">支付单数据</param>
        /// <returns></returns>
        public async Task<object> Refund(BillRefund refundInfo, BillPayments paymentInfo)
        {

            var weChatRefundUrl = AppSettingsConstVars.PayCallBackWeChatRefundUrl;
            if (string.IsNullOrEmpty(weChatRefundUrl))
            {
                throw new ApiException("未获取到配置的通知地址");
            }

            var request = new WeChatPayRefundRequest
            {
                OutRefundNo = refundInfo.RefundId,
                TransactionId = paymentInfo.TradeNo,
                OutTradeNo = paymentInfo.PaymentId,
                TotalFee = Convert.ToInt32(paymentInfo.Money * 100),
                RefundFee = Convert.ToInt32(refundInfo.Money * 100),
                NotifyUrl = weChatRefundUrl
            };
            var response = await _client.ExecuteAsync(request, _optionsAccessor.Value);

            if (response.ReturnCode == WeChatPayCode.Success && response.ResultCode == WeChatPayCode.Success)
            {
                return response;
            }
            else
            {
                throw new ApiException("退款失败");
            }
        }

        private static string GetTradeType(string tradeType)
        {
            if (tradeType != WeiChatPayTradeType.JSAPI.ToString() &&
                tradeType != WeiChatPayTradeType.JSAPI_OFFICIAL.ToString() &&
                tradeType != WeiChatPayTradeType.NATIVE.ToString() &&
                tradeType != WeiChatPayTradeType.APP.ToString() &&
                tradeType != WeiChatPayTradeType.MWEB.ToString()
            )
                return "JSAPI";
            if (tradeType == WeiChatPayTradeType.JSAPI_OFFICIAL.ToString())
                return "JSAPI";
            return tradeType;
        }
    }
}
