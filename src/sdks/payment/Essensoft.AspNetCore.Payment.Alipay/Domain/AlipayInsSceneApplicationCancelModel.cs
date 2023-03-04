using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayInsSceneApplicationCancelModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayInsSceneApplicationCancelModel : AlipayObject
    {
        /// <summary>
        /// 投保订单号
        /// </summary>
        [JsonProperty("application_no")]
        [XmlElement("application_no")]
        public string ApplicationNo { get; set; }
    }
}
