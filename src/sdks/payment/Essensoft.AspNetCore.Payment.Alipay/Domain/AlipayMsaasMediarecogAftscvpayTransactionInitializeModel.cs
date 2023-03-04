using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayMsaasMediarecogAftscvpayTransactionInitializeModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayMsaasMediarecogAftscvpayTransactionInitializeModel : AlipayObject
    {
        /// <summary>
        /// 商品信息,列表
        /// </summary>
        [JsonProperty("goods_infos")]
        [XmlArray("goods_infos")]
        [XmlArrayItem("good_info")]
        public List<GoodInfo> GoodsInfos { get; set; }

        /// <summary>
        /// 标识是什么场景下的视觉识别请求
        /// </summary>
        [JsonProperty("scene")]
        [XmlElement("scene")]
        public string Scene { get; set; }

        /// <summary>
        /// 售卖设备编号
        /// </summary>
        [JsonProperty("terminal_id")]
        [XmlElement("terminal_id")]
        public string TerminalId { get; set; }

        /// <summary>
        /// 云平台请求唯一标示,保证用户请求的幂等性.
        /// </summary>
        [JsonProperty("transaction_id")]
        [XmlElement("transaction_id")]
        public string TransactionId { get; set; }
    }
}
