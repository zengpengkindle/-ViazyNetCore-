using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEbppIsvProdmodeCreateModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEbppIsvProdmodeCreateModel : AlipayObject
    {
        /// <summary>
        /// 参数内容包含：ISV录入产品模型相关信息，具体分类如下：1.销账机构信息  2.对账配置信息  3.清算配置信息  4.产品模型和出账机构信息  5.机构拓展信息
        /// </summary>
        [JsonProperty("biz_data")]
        [XmlElement("biz_data")]
        public string BizData { get; set; }
    }
}
