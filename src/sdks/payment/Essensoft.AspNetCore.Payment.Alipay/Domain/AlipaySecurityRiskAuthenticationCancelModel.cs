using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipaySecurityRiskAuthenticationCancelModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipaySecurityRiskAuthenticationCancelModel : AlipayObject
    {
        /// <summary>
        /// 身份认证场景信息
        /// </summary>
        [JsonProperty("authentication_scene")]
        [XmlElement("authentication_scene")]
        public AuthenticationScene AuthenticationScene { get; set; }

        /// <summary>
        /// 业务流水号，与初始化接口保持一致
        /// </summary>
        [JsonProperty("biz_id")]
        [XmlElement("biz_id")]
        public string BizId { get; set; }

        /// <summary>
        /// 业务参数
        /// </summary>
        [JsonProperty("biz_info")]
        [XmlElement("biz_info")]
        public string BizInfo { get; set; }

        /// <summary>
        /// 身份认证初始化返回token_id
        /// </summary>
        [JsonProperty("token_id")]
        [XmlElement("token_id")]
        public string TokenId { get; set; }
    }
}
