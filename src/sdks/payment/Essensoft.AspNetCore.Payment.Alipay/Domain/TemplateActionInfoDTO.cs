using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// TemplateActionInfoDTO Data Structure.
    /// </summary>
    [Serializable]
    public class TemplateActionInfoDTO : AlipayObject
    {
        /// <summary>
        /// 行动点业务CODE，商户自定义
        /// </summary>
        [JsonProperty("code")]
        [XmlElement("code")]
        public string Code { get; set; }

        /// <summary>
        /// 小程序页面跳转链接，当url_type取"miniAppUrl"时必填
        /// </summary>
        [JsonProperty("mini_app_url")]
        [XmlElement("mini_app_url")]
        public TemplateActionMiniAppUrlDTO MiniAppUrl { get; set; }

        /// <summary>
        /// 行动点展示文案
        /// </summary>
        [JsonProperty("text")]
        [XmlElement("text")]
        public string Text { get; set; }

        /// <summary>
        /// 行动点跳转链接，当url_type取"url"或不填时必填
        /// </summary>
        [JsonProperty("url")]
        [XmlElement("url")]
        public string Url { get; set; }

        /// <summary>
        /// 跳转链接类型，当前支持url和miniAppUrl，不填则默认为url类型：  url：对应填写url参数，支持http(s)和支付宝schema地址等；  miniAppUrl: 对应填写mini_app_url参数，跳转至指定的支付宝小程序页面
        /// </summary>
        [JsonProperty("url_type")]
        [XmlElement("url_type")]
        public string UrlType { get; set; }
    }
}
