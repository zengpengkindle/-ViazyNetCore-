using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipaySocialBaseChatGmemberDeleteModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipaySocialBaseChatGmemberDeleteModel : AlipayObject
    {
        /// <summary>
        /// 群id
        /// </summary>
        [JsonProperty("group_id")]
        [XmlElement("group_id")]
        public string GroupId { get; set; }

        /// <summary>
        /// 剔除的群成员用户id列表
        /// </summary>
        [JsonProperty("uids")]
        [XmlArray("uids")]
        [XmlArrayItem("string")]
        public List<string> Uids { get; set; }
    }
}
