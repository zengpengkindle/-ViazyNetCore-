using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEcoRenthouseKaBaseinfoQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEcoRenthouseKaBaseinfoQueryModel : AlipayObject
    {
        /// <summary>
        /// kaCode唯一标识
        /// </summary>
        [JsonProperty("ka_code")]
        [XmlElement("ka_code")]
        public string KaCode { get; set; }
    }
}
