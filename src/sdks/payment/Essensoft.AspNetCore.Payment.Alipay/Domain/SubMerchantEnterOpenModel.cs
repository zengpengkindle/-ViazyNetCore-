using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// SubMerchantEnterOpenModel Data Structure.
    /// </summary>
    [Serializable]
    public class SubMerchantEnterOpenModel : AlipayObject
    {
        /// <summary>
        /// 开票商户pid，入驻支付即开票场景的时候，该字段必传
        /// </summary>
        [JsonProperty("pid")]
        [XmlElement("pid")]
        public string Pid { get; set; }

        /// <summary>
        /// 商户门店税号。
        /// </summary>
        [JsonProperty("register_no")]
        [XmlElement("register_no")]
        public string RegisterNo { get; set; }

        /// <summary>
        /// 商户门店全称。
        /// </summary>
        [JsonProperty("sub_m_name")]
        [XmlElement("sub_m_name")]
        public string SubMName { get; set; }

        /// <summary>
        /// 商户门店简称，只能由大写字母、下划线、数字组成，且必须以大写字母开头。
        /// </summary>
        [JsonProperty("sub_m_short_name")]
        [XmlElement("sub_m_short_name")]
        public string SubMShortName { get; set; }
    }
}
