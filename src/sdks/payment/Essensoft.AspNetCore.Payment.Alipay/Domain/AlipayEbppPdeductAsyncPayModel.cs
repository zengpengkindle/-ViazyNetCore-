using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEbppPdeductAsyncPayModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEbppPdeductAsyncPayModel : AlipayObject
    {
        /// <summary>
        /// 分配给外部机构发起扣款时的渠道码。朗新为LANGXIN
        /// </summary>
        [JsonProperty("agent_channel")]
        [XmlElement("agent_channel")]
        public string AgentChannel { get; set; }

        /// <summary>
        /// 二级渠道码，预留字段
        /// </summary>
        [JsonProperty("agent_code")]
        [XmlElement("agent_code")]
        public string AgentCode { get; set; }

        /// <summary>
        /// 支付宝代扣协议Id
        /// </summary>
        [JsonProperty("agreement_id")]
        [XmlElement("agreement_id")]
        public string AgreementId { get; set; }

        /// <summary>
        /// 账期
        /// </summary>
        [JsonProperty("bill_date")]
        [XmlElement("bill_date")]
        public string BillDate { get; set; }

        /// <summary>
        /// 户号
        /// </summary>
        [JsonProperty("bill_key")]
        [XmlElement("bill_key")]
        public string BillKey { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        [JsonProperty("extend_field")]
        [XmlElement("extend_field")]
        public string ExtendField { get; set; }

        /// <summary>
        /// 滞纳金
        /// </summary>
        [JsonProperty("fine_amount")]
        [XmlElement("fine_amount")]
        public string FineAmount { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [JsonProperty("memo")]
        [XmlElement("memo")]
        public string Memo { get; set; }

        /// <summary>
        /// 商户外部业务流水号
        /// </summary>
        [JsonProperty("out_order_no")]
        [XmlElement("out_order_no")]
        public string OutOrderNo { get; set; }

        /// <summary>
        /// 扣款金额，支付总金额，包含滞纳金
        /// </summary>
        [JsonProperty("pay_amount")]
        [XmlElement("pay_amount")]
        public string PayAmount { get; set; }

        /// <summary>
        /// 商户partnerId
        /// </summary>
        [JsonProperty("pid")]
        [XmlElement("pid")]
        public string Pid { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty("user_id")]
        [XmlElement("user_id")]
        public string UserId { get; set; }
    }
}
