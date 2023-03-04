using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayOpenServicemarketCommodityExtendinfosConfirmModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayOpenServicemarketCommodityExtendinfosConfirmModel : AlipayObject
    {
        /// <summary>
        /// 公服BD审核扩展信息
        /// </summary>
        [JsonProperty("commodity_ext_infos")]
        [XmlArray("commodity_ext_infos")]
        [XmlArrayItem("commodity_ext_info_confirm")]
        public List<CommodityExtInfoConfirm> CommodityExtInfos { get; set; }

        /// <summary>
        /// 服务Id
        /// </summary>
        [JsonProperty("commodity_id")]
        [XmlElement("commodity_id")]
        public string CommodityId { get; set; }

        /// <summary>
        /// status 为驳回时 必须输入驳回原因
        /// </summary>
        [JsonProperty("memo")]
        [XmlElement("memo")]
        public string Memo { get; set; }

        /// <summary>
        /// 公服BD审核结果：成功还是失败:  status 【0：表示不通过 ， 1：表示通过】
        /// </summary>
        [JsonProperty("status")]
        [XmlElement("status")]
        public string Status { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonProperty("user_id")]
        [XmlElement("user_id")]
        public string UserId { get; set; }
    }
}
