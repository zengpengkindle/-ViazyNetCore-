using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Response
{
    /// <summary>
    /// AlipayPassSyncUpdateResponse.
    /// </summary>
    public class AlipayPassSyncUpdateResponse : AlipayResponse
    {
        /// <summary>
        /// 更新成功时的业务信息
        /// </summary>
        [JsonProperty("biz_result")]
        [XmlElement("biz_result")]
        public string BizResult { get; set; }

        /// <summary>
        /// 当新增alipass不成功时，产生的错误码.
        /// </summary>
        [JsonProperty("error_code")]
        [XmlElement("error_code")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 是否更新成功的标识。
        /// </summary>
        [JsonProperty("success")]
        [XmlElement("success")]
        public bool Success { get; set; }
    }
}
