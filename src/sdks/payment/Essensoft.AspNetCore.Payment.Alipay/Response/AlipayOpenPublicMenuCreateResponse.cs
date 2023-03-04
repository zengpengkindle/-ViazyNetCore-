using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Response
{
    /// <summary>
    /// AlipayOpenPublicMenuCreateResponse.
    /// </summary>
    public class AlipayOpenPublicMenuCreateResponse : AlipayResponse
    {
        /// <summary>
        /// 默认菜单菜单key，文本菜单为“default”，icon菜单为“iconDefault”
        /// </summary>
        [JsonProperty("menu_key")]
        [XmlElement("menu_key")]
        public string MenuKey { get; set; }
    }
}
