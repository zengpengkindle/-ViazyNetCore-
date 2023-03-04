using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// OverseasTravelRate Data Structure.
    /// </summary>
    [Serializable]
    public class OverseasTravelRate : AlipayObject
    {
        /// <summary>
        /// 货币代码，ISO标准alpha- 3币种代码
        /// </summary>
        [JsonProperty("currency")]
        [XmlElement("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// 货币icon的url地址
        /// </summary>
        [JsonProperty("currency_icon")]
        [XmlElement("currency_icon")]
        public string CurrencyIcon { get; set; }

        /// <summary>
        /// 汇率，double类型，为支付宝当面付的当前币种/CNY的汇率值，如1美元=6.2345人民币元，即汇率为6.2345
        /// </summary>
        [JsonProperty("rate")]
        [XmlElement("rate")]
        public string Rate { get; set; }
    }
}
