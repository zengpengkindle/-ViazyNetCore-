using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEcoEduCampusJobPublishModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEcoEduCampusJobPublishModel : AlipayObject
    {
        /// <summary>
        /// 过期时间(毫秒数)
        /// </summary>
        [JsonProperty("gmt_expired")]
        [XmlElement("gmt_expired")]
        public string GmtExpired { get; set; }

        /// <summary>
        /// 刷新时间(毫秒数)
        /// </summary>
        [JsonProperty("gmt_refresh")]
        [XmlElement("gmt_refresh")]
        public string GmtRefresh { get; set; }

        /// <summary>
        /// 职位来源方编码
        /// </summary>
        [JsonProperty("source_code")]
        [XmlElement("source_code")]
        public string SourceCode { get; set; }

        /// <summary>
        /// 职位在合作方的ID
        /// </summary>
        [JsonProperty("source_id")]
        [XmlElement("source_id")]
        public string SourceId { get; set; }
    }
}
