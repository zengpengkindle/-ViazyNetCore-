using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayDataDataserviceLbsDiscountBatchqueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayDataDataserviceLbsDiscountBatchqueryModel : AlipayObject
    {
        /// <summary>
        /// 媒体编号，使用前需要找业务申请 ，不申请直接调用会失败
        /// </summary>
        [JsonProperty("channel")]
        [XmlElement("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// 手机IMEI号，imei、user_id、mobile三者必须且只能填一个
        /// </summary>
        [JsonProperty("imei")]
        [XmlElement("imei")]
        public string Imei { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [JsonProperty("latitude")]
        [XmlElement("latitude")]
        public string Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [JsonProperty("longitude")]
        [XmlElement("longitude")]
        public string Longitude { get; set; }

        /// <summary>
        /// 手机号码，imei、user_id、mobile三者必须且只能填一个
        /// </summary>
        [JsonProperty("mobile")]
        [XmlElement("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// 输出的券列表大小
        /// </summary>
        [JsonProperty("size")]
        [XmlElement("size")]
        public long Size { get; set; }

        /// <summary>
        /// 支付宝用户ID，imei、user_id、mobile三者必须且只能填一个
        /// </summary>
        [JsonProperty("user_id")]
        [XmlElement("user_id")]
        public string UserId { get; set; }
    }
}
