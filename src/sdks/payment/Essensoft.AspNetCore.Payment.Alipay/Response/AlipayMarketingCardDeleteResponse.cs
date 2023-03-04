using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Response
{
    /// <summary>
    /// AlipayMarketingCardDeleteResponse.
    /// </summary>
    public class AlipayMarketingCardDeleteResponse : AlipayResponse
    {
        /// <summary>
        /// 支付宝端删卡业务流水号
        /// </summary>
        [JsonProperty("biz_serial_no")]
        [XmlElement("biz_serial_no")]
        public string BizSerialNo { get; set; }
    }
}
