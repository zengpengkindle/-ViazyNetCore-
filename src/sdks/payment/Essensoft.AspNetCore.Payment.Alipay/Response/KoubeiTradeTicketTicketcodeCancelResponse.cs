using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Response
{
    /// <summary>
    /// KoubeiTradeTicketTicketcodeCancelResponse.
    /// </summary>
    public class KoubeiTradeTicketTicketcodeCancelResponse : AlipayResponse
    {
        /// <summary>
        /// 该字段用于描述本次返回中的业务属性，现有：BIZ_ALREADY_SUCCESS（幂等业务码）。
        /// </summary>
        [JsonProperty("biz_code")]
        [XmlElement("biz_code")]
        public string BizCode { get; set; }

        /// <summary>
        /// 外部请求号，支持英文字母和数字，由开发者自行定义（不允许重复）
        /// </summary>
        [JsonProperty("request_id")]
        [XmlElement("request_id")]
        public string RequestId { get; set; }
    }
}
