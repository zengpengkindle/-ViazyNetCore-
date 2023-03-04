using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEcoBasicRouterSendModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEcoBasicRouterSendModel : AlipayObject
    {
        /// <summary>
        /// 路由数据
        /// </summary>
        [JsonProperty("input")]
        [XmlElement("input")]
        public string Input { get; set; }

        /// <summary>
        /// 路由任务名
        /// </summary>
        [JsonProperty("interface_name")]
        [XmlElement("interface_name")]
        public string InterfaceName { get; set; }

        /// <summary>
        /// 本次调用唯一id
        /// </summary>
        [JsonProperty("trace_id")]
        [XmlElement("trace_id")]
        public string TraceId { get; set; }
    }
}
