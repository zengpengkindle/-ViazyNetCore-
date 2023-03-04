using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayBossBaseProcessInstanceCancelModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayBossBaseProcessInstanceCancelModel : AlipayObject
    {
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("memo")]
        [XmlElement("memo")]
        public string Memo { get; set; }

        /// <summary>
        /// 处理人域账号
        /// </summary>
        [JsonProperty("operator")]
        [XmlElement("operator")]
        public string Operator { get; set; }

        /// <summary>
        /// 流程全局唯一ID
        /// </summary>
        [JsonProperty("puid")]
        [XmlElement("puid")]
        public string Puid { get; set; }
    }
}
