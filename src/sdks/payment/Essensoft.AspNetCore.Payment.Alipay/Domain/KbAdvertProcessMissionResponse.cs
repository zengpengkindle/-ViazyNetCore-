using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// KbAdvertProcessMissionResponse Data Structure.
    /// </summary>
    [Serializable]
    public class KbAdvertProcessMissionResponse : AlipayObject
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        [JsonProperty("identify")]
        [XmlElement("identify")]
        public string Identify { get; set; }

        /// <summary>
        /// 主键类型  activity_id：运营活动ID  voucher：商品ID  mission：分佣任务ID
        /// </summary>
        [JsonProperty("identify_type")]
        [XmlElement("identify_type")]
        public string IdentifyType { get; set; }

        /// <summary>
        /// 任务状态  UNCONFIRMED-未确认（代表任务还在等待商户确认）  EFFECTIVE-有效  INVALID-无效
        /// </summary>
        [JsonProperty("promote_status")]
        [XmlElement("promote_status")]
        public string PromoteStatus { get; set; }
    }
}
