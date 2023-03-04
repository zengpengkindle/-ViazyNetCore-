using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Response
{
    /// <summary>
    /// AlipayFundAuthOrderAppFreezeResponse.
    /// </summary>
    public class AlipayFundAuthOrderAppFreezeResponse : AlipayResponse
    {
        /// <summary>
        /// 本次操作冻结的金额，单位为：元（人民币），精确到小数点后两位
        /// </summary>
        [JsonProperty("amount")]
        [XmlElement("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// 支付宝的资金授权订单号
        /// </summary>
        [JsonProperty("auth_no")]
        [XmlElement("auth_no")]
        public string AuthNo { get; set; }

        /// <summary>
        /// 本次冻结操作中信用冻结金额，单位为：元（人民币），精确到小数点后两位
        /// </summary>
        [JsonProperty("credit_amount")]
        [XmlElement("credit_amount")]
        public string CreditAmount { get; set; }

        /// <summary>
        /// 本次冻结操作中自有资金冻结金额，单位为：元（人民币），精确到小数点后两位
        /// </summary>
        [JsonProperty("fund_amount")]
        [XmlElement("fund_amount")]
        public string FundAmount { get; set; }

        /// <summary>
        /// 资金授权成功时间  格式：YYYY-MM-DD HH:MM:SS
        /// </summary>
        [JsonProperty("gmt_trans")]
        [XmlElement("gmt_trans")]
        public string GmtTrans { get; set; }

        /// <summary>
        /// 支付宝的资金操作流水号
        /// </summary>
        [JsonProperty("operation_id")]
        [XmlElement("operation_id")]
        public string OperationId { get; set; }

        /// <summary>
        /// 商户的授权资金订单号
        /// </summary>
        [JsonProperty("out_order_no")]
        [XmlElement("out_order_no")]
        public string OutOrderNo { get; set; }

        /// <summary>
        /// 商户本次资金操作的请求流水号
        /// </summary>
        [JsonProperty("out_request_no")]
        [XmlElement("out_request_no")]
        public string OutRequestNo { get; set; }

        /// <summary>
        /// 付款方支付宝用户号
        /// </summary>
        [JsonProperty("payer_user_id")]
        [XmlElement("payer_user_id")]
        public string PayerUserId { get; set; }

        /// <summary>
        /// 预授权类型，目前支持 CREDIT_AUTH(信用预授权);  商户可根据该标识来判断该笔预授权的类型，当返回值为"CREDIT_AUTH"表明该笔预授权为信用预授权，没有真实冻结资金；当返回值为空或者不为"CREDIT_AUTH"则表明该笔预授权为普通资金预授权，会冻结用户资金。
        /// </summary>
        [JsonProperty("pre_auth_type")]
        [XmlElement("pre_auth_type")]
        public string PreAuthType { get; set; }

        /// <summary>
        /// 资金预授权明细的状态  目前支持：    INIT：初始  SUCCESS: 成功  CLOSED：关闭
        /// </summary>
        [JsonProperty("status")]
        [XmlElement("status")]
        public string Status { get; set; }
    }
}
