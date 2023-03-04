using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// KoubeiRetailWmsOutboundorderBatchqueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class KoubeiRetailWmsOutboundorderBatchqueryModel : AlipayObject
    {
        /// <summary>
        /// 操作人信息
        /// </summary>
        [JsonProperty("operate_context")]
        [XmlElement("operate_context")]
        public OperateContext OperateContext { get; set; }

        /// <summary>
        /// 外部业务单号（出库通知单id列表列表二选一）
        /// </summary>
        [JsonProperty("out_biz_no_list")]
        [XmlArray("out_biz_no_list")]
        [XmlArrayItem("string")]
        public List<string> OutBizNoList { get; set; }

        /// <summary>
        /// 出库通知单id列表（与外部业务单号列表二选一）
        /// </summary>
        [JsonProperty("outbound_order_id_list")]
        [XmlArray("outbound_order_id_list")]
        [XmlArrayItem("string")]
        public List<string> OutboundOrderIdList { get; set; }
    }
}
