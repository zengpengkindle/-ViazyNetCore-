using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayMarketingCampaignRuleCrowdDeleteModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayMarketingCampaignRuleCrowdDeleteModel : AlipayObject
    {
        /// <summary>
        /// 签约商户下属子机构唯一编号
        /// </summary>
        [JsonProperty("mpid")]
        [XmlElement("mpid")]
        public string Mpid { get; set; }

        /// <summary>
        /// 对没有在使用的规则进行删除
        /// </summary>
        [JsonProperty("ruleid")]
        [XmlElement("ruleid")]
        public string Ruleid { get; set; }
    }
}
