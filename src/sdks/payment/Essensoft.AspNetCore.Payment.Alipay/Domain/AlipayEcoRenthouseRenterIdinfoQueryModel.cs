using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEcoRenthouseRenterIdinfoQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEcoRenthouseRenterIdinfoQueryModel : AlipayObject
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        [JsonProperty("card_no")]
        [XmlElement("card_no")]
        public string CardNo { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [JsonProperty("user_name")]
        [XmlElement("user_name")]
        public string UserName { get; set; }
    }
}
