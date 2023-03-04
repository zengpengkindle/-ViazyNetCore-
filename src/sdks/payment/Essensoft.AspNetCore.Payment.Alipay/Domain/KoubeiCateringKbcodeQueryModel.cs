using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// KoubeiCateringKbcodeQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class KoubeiCateringKbcodeQueryModel : AlipayObject
    {
        /// <summary>
        /// 创建口碑码时返回的批次号（batch_id和shop_id二者必填其一）
        /// </summary>
        [JsonProperty("batch_id")]
        [XmlElement("batch_id")]
        public long BatchId { get; set; }

        /// <summary>
        /// 当前页码（大于0的整数）
        /// </summary>
        [JsonProperty("page_num")]
        [XmlElement("page_num")]
        public long PageNum { get; set; }

        /// <summary>
        /// 每页返回的记录数（0~100的整数）
        /// </summary>
        [JsonProperty("page_size")]
        [XmlElement("page_size")]
        public long PageSize { get; set; }

        /// <summary>
        /// 口碑店铺ID（batch_id和shop_id二者必填其一）
        /// </summary>
        [JsonProperty("shop_id")]
        [XmlElement("shop_id")]
        public string ShopId { get; set; }
    }
}
