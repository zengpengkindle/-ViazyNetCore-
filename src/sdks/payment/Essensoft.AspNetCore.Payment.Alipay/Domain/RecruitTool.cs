using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// RecruitTool Data Structure.
    /// </summary>
    [Serializable]
    public class RecruitTool : AlipayObject
    {
        /// <summary>
        /// 招商结束时间
        /// </summary>
        [JsonProperty("end_time")]
        [XmlElement("end_time")]
        public string EndTime { get; set; }

        /// <summary>
        /// 如果这个值是true,那么活动的参与门店不需要招商
        /// </summary>
        [JsonProperty("exclude_constraint_shops")]
        [XmlElement("exclude_constraint_shops")]
        public bool ExcludeConstraintShops { get; set; }

        /// <summary>
        /// 招商pid和pid对应的门店列表（对于品牌商，此字段必填，活动和券的适用门店为空。对于商圈，此字段需为空，门店需要填在活动和券的适用门店上）
        /// </summary>
        [JsonProperty("pid_shops")]
        [XmlArray("pid_shops")]
        [XmlArrayItem("pid_shop_info")]
        public List<PidShopInfo> PidShops { get; set; }

        /// <summary>
        /// 招商开始时间
        /// </summary>
        [JsonProperty("start_time")]
        [XmlElement("start_time")]
        public string StartTime { get; set; }
    }
}
