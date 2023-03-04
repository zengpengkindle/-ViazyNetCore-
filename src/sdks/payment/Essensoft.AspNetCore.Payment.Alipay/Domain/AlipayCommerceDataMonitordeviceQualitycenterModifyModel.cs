using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayCommerceDataMonitordeviceQualitycenterModifyModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayCommerceDataMonitordeviceQualitycenterModifyModel : AlipayObject
    {
        /// <summary>
        /// device_id, 唯一标识设备，由openmonitor传递到质量中心。
        /// </summary>
        [JsonProperty("asset_id")]
        [XmlElement("asset_id")]
        public string AssetId { get; set; }

        /// <summary>
        /// fault_type，标识设备故障类型，由openmonitor传递到质量中心。
        /// </summary>
        [JsonProperty("fault_type")]
        [XmlElement("fault_type")]
        public string FaultType { get; set; }

        /// <summary>
        /// invoke_id, 用来回溯监控中心的调用方id，监控中心生成
        /// </summary>
        [JsonProperty("invoke_id")]
        [XmlElement("invoke_id")]
        public long InvokeId { get; set; }

        /// <summary>
        /// quality_center_id，唯一，用于问题追踪去重，质量中心生成的唯一id。
        /// </summary>
        [JsonProperty("quality_center_id")]
        [XmlElement("quality_center_id")]
        public string QualityCenterId { get; set; }

        /// <summary>
        /// result, 机具处理结果，由质量中心生成。
        /// </summary>
        [JsonProperty("result")]
        [XmlElement("result")]
        public string Result { get; set; }

        /// <summary>
        /// result_date, 处理结果的时间，质量中心生成。
        /// </summary>
        [JsonProperty("result_date")]
        [XmlElement("result_date")]
        public string ResultDate { get; set; }

        /// <summary>
        /// result_msg，处理结果的描述，质量中心生成。
        /// </summary>
        [JsonProperty("result_msg")]
        [XmlElement("result_msg")]
        public string ResultMsg { get; set; }
    }
}
