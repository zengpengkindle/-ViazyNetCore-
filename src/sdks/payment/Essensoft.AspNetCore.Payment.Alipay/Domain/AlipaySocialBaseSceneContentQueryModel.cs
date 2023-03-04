using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipaySocialBaseSceneContentQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipaySocialBaseSceneContentQueryModel : AlipayObject
    {
        /// <summary>
        /// 城市id
        /// </summary>
        [JsonProperty("city_id")]
        [XmlElement("city_id")]
        public string CityId { get; set; }

        /// <summary>
        /// 内容中台提供的运营后台配置场景id
        /// </summary>
        [JsonProperty("scene_id")]
        [XmlElement("scene_id")]
        public string SceneId { get; set; }

        /// <summary>
        /// 返回文章列表的个数，目前最多10条
        /// </summary>
        [JsonProperty("top_size")]
        [XmlElement("top_size")]
        public long TopSize { get; set; }

        /// <summary>
        /// 蚂蚁统一会员ID
        /// </summary>
        [JsonProperty("user_id")]
        [XmlElement("user_id")]
        public string UserId { get; set; }
    }
}
