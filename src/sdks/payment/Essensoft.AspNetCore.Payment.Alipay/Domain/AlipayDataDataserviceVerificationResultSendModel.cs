using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayDataDataserviceVerificationResultSendModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayDataDataserviceVerificationResultSendModel : AlipayObject
    {
        /// <summary>
        /// 部署单记录id
        /// </summary>
        [JsonProperty("record_id")]
        [XmlElement("record_id")]
        public long RecordId { get; set; }

        /// <summary>
        /// 验证结果列表
        /// </summary>
        [JsonProperty("result_items")]
        [XmlArray("result_items")]
        [XmlArrayItem("dx_verify_result_item")]
        public List<DxVerifyResultItem> ResultItems { get; set; }
    }
}
