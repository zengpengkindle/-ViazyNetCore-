using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// KoubeiCateringOrderPushSignModel Data Structure.
    /// </summary>
    [Serializable]
    public class KoubeiCateringOrderPushSignModel : AlipayObject
    {
        /// <summary>
        /// 口碑推单批次号，口碑推单时传入；
        /// </summary>
        [JsonProperty("batch_no")]
        [XmlElement("batch_no")]
        public string BatchNo { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        [JsonProperty("ext_infos")]
        [XmlElement("ext_infos")]
        public string ExtInfos { get; set; }

        /// <summary>
        /// 口碑主订单号
        /// </summary>
        [JsonProperty("order_id")]
        [XmlElement("order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// POS订单号；在接单的情况下，必须返回
        /// </summary>
        [JsonProperty("out_biz_no")]
        [XmlElement("out_biz_no")]
        public string OutBizNo { get; set; }

        /// <summary>
        /// 推单处理结果，可枚举：  (1) 已拒单：REJECT  (2) 已接单：RECEIPT
        /// </summary>
        [JsonProperty("receipt_code")]
        [XmlElement("receipt_code")]
        public string ReceiptCode { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        [JsonProperty("receipt_time")]
        [XmlElement("receipt_time")]
        public string ReceiptTime { get; set; }

        /// <summary>
        /// 拒单原因编码：  (1) 桌号不存在，请联系服务员：TABLE_NOT_EXIST  (2) 店铺太忙，无法接待：BUSY  (3) 重复订单：DUPLICATE_ORDER  (4) 店铺已打烊：SHOP_CLOSE  (5) 菜品售完：SELL_OUT  (6) 其他原因：OTHER_REASON
        /// </summary>
        [JsonProperty("reject_reason_code")]
        [XmlElement("reject_reason_code")]
        public string RejectReasonCode { get; set; }

        /// <summary>
        /// 拒单的原因描述
        /// </summary>
        [JsonProperty("reject_reason_desc")]
        [XmlElement("reject_reason_desc")]
        public string RejectReasonDesc { get; set; }
    }
}
