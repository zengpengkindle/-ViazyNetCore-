using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// SettleDetailInfo Data Structure.
    /// </summary>
    [Serializable]
    public class SettleDetailInfo : AlipayObject
    {
        /// <summary>
        /// 结算的金额，单位为元。目前必须和交易金额相同
        /// </summary>
        [JsonProperty("amount")]
        [XmlElement("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// 结算主体标识。当结算主体类型为SecondMerchant时，为二级商户的SecondMerchantID；当结算主体类型为Store时，为门店的外标。
        /// </summary>
        [JsonProperty("settle_entity_id")]
        [XmlElement("settle_entity_id")]
        public string SettleEntityId { get; set; }

        /// <summary>
        /// 结算主体类型。  二级商户:SecondMerchant;商户或者直连商户门店:Store
        /// </summary>
        [JsonProperty("settle_entity_type")]
        [XmlElement("settle_entity_type")]
        public string SettleEntityType { get; set; }

        /// <summary>
        /// 结算汇总维度，按照这个维度汇总成批次结算，由商户指定。    目前需要和结算收款方账户类型为cardSerialNo配合使用
        /// </summary>
        [JsonProperty("summary_dimension")]
        [XmlElement("summary_dimension")]
        public string SummaryDimension { get; set; }

        /// <summary>
        /// 结算收款方。当结算收款方类型是cardSerialNo时，本参数为用户在支付宝绑定的卡编号；结算收款方类型是userId时，本参数为用户的支付宝账号对应的支付宝唯一用户号，以2088开头的纯16位数字；当结算收款方类型是loginName时，本参数为用户的支付宝登录号
        /// </summary>
        [JsonProperty("trans_in")]
        [XmlElement("trans_in")]
        public string TransIn { get; set; }

        /// <summary>
        /// 结算收款方的账户类型。    cardSerialNo：结算收款方的银行卡编号;    userId：表示是支付宝账号对应的支付宝唯一用户号;  loginName：表示是支付宝登录号；
        /// </summary>
        [JsonProperty("trans_in_type")]
        [XmlElement("trans_in_type")]
        public string TransInType { get; set; }
    }
}
