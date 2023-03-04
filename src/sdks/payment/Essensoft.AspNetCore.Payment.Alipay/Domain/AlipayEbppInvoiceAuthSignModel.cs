using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEbppInvoiceAuthSignModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEbppInvoiceAuthSignModel : AlipayObject
    {
        /// <summary>
        /// 发票授权类型，可选值：INVOICE_AUTO_SYNC（发票自动回传）
        /// </summary>
        [JsonProperty("authorization_type")]
        [XmlElement("authorization_type")]
        public string AuthorizationType { get; set; }

        /// <summary>
        /// 开票商户品牌简称，与商户入驻时的品牌简称保持一致。
        /// </summary>
        [JsonProperty("m_short_name")]
        [XmlElement("m_short_name")]
        public string MShortName { get; set; }

        /// <summary>
        /// 支付宝用户userId
        /// </summary>
        [JsonProperty("user_id")]
        [XmlElement("user_id")]
        public string UserId { get; set; }
    }
}
