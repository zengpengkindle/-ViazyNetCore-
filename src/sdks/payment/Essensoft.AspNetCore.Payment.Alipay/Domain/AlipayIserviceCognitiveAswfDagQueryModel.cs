using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayIserviceCognitiveAswfDagQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayIserviceCognitiveAswfDagQueryModel : AlipayObject
    {
        /// <summary>
        /// 业务唯一标识，不可空
        /// </summary>
        [JsonProperty("biz_id")]
        [XmlElement("biz_id")]
        public string BizId { get; set; }

        /// <summary>
        /// 业务的任务描述
        /// </summary>
        [JsonProperty("ctxs")]
        [XmlElement("ctxs")]
        public string Ctxs { get; set; }

        /// <summary>
        /// DAG模板ID，不可空
        /// </summary>
        [JsonProperty("template_id")]
        [XmlElement("template_id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 业务请求唯一id
        /// </summary>
        [JsonProperty("trace_id")]
        [XmlElement("trace_id")]
        public string TraceId { get; set; }
    }
}
