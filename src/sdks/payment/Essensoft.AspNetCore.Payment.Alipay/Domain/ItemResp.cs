using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// ItemResp Data Structure.
    /// </summary>
    [Serializable]
    public class ItemResp : AlipayObject
    {
        /// <summary>
        /// 内容分类
        /// </summary>
        [JsonProperty("category")]
        [XmlElement("category")]
        public string Category { get; set; }

        /// <summary>
        /// 用户是否已购买或已领取:true-已购买/已领取,false-未购买/未领取
        /// </summary>
        [JsonProperty("has_recive")]
        [XmlElement("has_recive")]
        public bool HasRecive { get; set; }

        /// <summary>
        /// 小图标地址
        /// </summary>
        [JsonProperty("icon")]
        [XmlElement("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 营销图片
        /// </summary>
        [JsonProperty("image")]
        [XmlElement("image")]
        public string Image { get; set; }

        /// <summary>
        /// 商品、券ID
        /// </summary>
        [JsonProperty("item_id")]
        [XmlElement("item_id")]
        public string ItemId { get; set; }

        /// <summary>
        /// 内容意义
        /// </summary>
        [JsonProperty("meaning")]
        [XmlElement("meaning")]
        public string Meaning { get; set; }

        /// <summary>
        /// 商品价格或折扣
        /// </summary>
        [JsonProperty("price")]
        [XmlElement("price")]
        public string Price { get; set; }

        /// <summary>
        /// 简要描述
        /// </summary>
        [JsonProperty("summary")]
        [XmlElement("summary")]
        public string Summary { get; set; }

        /// <summary>
        /// 内容标签
        /// </summary>
        [JsonProperty("tags")]
        [XmlElement("tags")]
        public string Tags { get; set; }

        /// <summary>
        /// 限制及特性描述
        /// </summary>
        [JsonProperty("tips")]
        [XmlElement("tips")]
        public string Tips { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// 类型:VOUCHER(券)、ITEM(商品)
        /// </summary>
        [JsonProperty("type")]
        [XmlElement("type")]
        public string Type { get; set; }

        /// <summary>
        /// 内容跳转地址
        /// </summary>
        [JsonProperty("url")]
        [XmlElement("url")]
        public string Url { get; set; }
    }
}
