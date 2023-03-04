using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// KbAdvertChannelResponse Data Structure.
    /// </summary>
    [Serializable]
    public class KbAdvertChannelResponse : AlipayObject
    {
        /// <summary>
        /// 渠道ID
        /// </summary>
        [JsonProperty("channel_id")]
        [XmlElement("channel_id")]
        public string ChannelId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("memo")]
        [XmlElement("memo")]
        public string Memo { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 渠道状态  EFFECTIVE：有效  INVALID：无效
        /// </summary>
        [JsonProperty("status")]
        [XmlElement("status")]
        public string Status { get; set; }

        /// <summary>
        /// OFFLINE：线下推广
        /// </summary>
        [JsonProperty("type")]
        [XmlElement("type")]
        public string Type { get; set; }
    }
}
