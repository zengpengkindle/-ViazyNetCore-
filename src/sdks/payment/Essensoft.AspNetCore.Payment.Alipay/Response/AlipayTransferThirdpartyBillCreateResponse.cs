using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Response
{
    /// <summary>
    /// AlipayTransferThirdpartyBillCreateResponse.
    /// </summary>
    public class AlipayTransferThirdpartyBillCreateResponse : AlipayResponse
    {
        /// <summary>
        /// 支付宝转账交易号
        /// </summary>
        [JsonProperty("order_id")]
        [XmlElement("order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// 交易类型，固定为transfer
        /// </summary>
        [JsonProperty("order_type")]
        [XmlElement("order_type")]
        public string OrderType { get; set; }

        /// <summary>
        /// 外部应用创建的交易ID
        /// </summary>
        [JsonProperty("payment_id")]
        [XmlElement("payment_id")]
        public string PaymentId { get; set; }
    }
}
