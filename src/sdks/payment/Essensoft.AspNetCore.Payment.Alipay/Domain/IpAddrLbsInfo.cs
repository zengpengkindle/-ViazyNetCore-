using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// IpAddrLbsInfo Data Structure.
    /// </summary>
    [Serializable]
    public class IpAddrLbsInfo : AlipayObject
    {
        /// <summary>
        /// IP地址归属地所以城市
        /// </summary>
        [JsonProperty("city")]
        [XmlElement("city")]
        public string City { get; set; }

        /// <summary>
        /// IP归属地所在的省
        /// </summary>
        [JsonProperty("province")]
        [XmlElement("province")]
        public string Province { get; set; }
    }
}
