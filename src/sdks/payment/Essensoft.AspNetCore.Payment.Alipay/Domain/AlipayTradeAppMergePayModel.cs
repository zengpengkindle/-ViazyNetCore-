using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayTradeAppMergePayModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayTradeAppMergePayModel : AlipayObject
    {
        /// <summary>
        /// 如果预创建成功，支付宝返回该预下单号，后续商户使用该预下单号请求支付宝支付接口
        /// </summary>
        [JsonProperty("pre_order_no")]
        [XmlElement("pre_order_no")]
        public string PreOrderNo { get; set; }
    }
}
