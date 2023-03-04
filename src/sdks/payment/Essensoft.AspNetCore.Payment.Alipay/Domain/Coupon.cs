using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// Coupon Data Structure.
    /// </summary>
    [Serializable]
    public class Coupon : AlipayObject
    {
        /// <summary>
        /// 当前可用面额
        /// </summary>
        [JsonProperty("available_amount")]
        [XmlElement("available_amount")]
        public string AvailableAmount { get; set; }

        /// <summary>
        /// 红包编号
        /// </summary>
        [JsonProperty("coupon_no")]
        [XmlElement("coupon_no")]
        public string CouponNo { get; set; }

        /// <summary>
        /// 可优惠面额
        /// </summary>
        [JsonProperty("deduct_amount")]
        [XmlElement("deduct_amount")]
        public string DeductAmount { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        [JsonProperty("gmt_active")]
        [XmlElement("gmt_active")]
        public string GmtActive { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("gmt_create")]
        [XmlElement("gmt_create")]
        public string GmtCreate { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        [JsonProperty("gmt_expired")]
        [XmlElement("gmt_expired")]
        public string GmtExpired { get; set; }

        /// <summary>
        /// 红包使用说明
        /// </summary>
        [JsonProperty("instructions")]
        [XmlElement("instructions")]
        public string Instructions { get; set; }

        /// <summary>
        /// 红包详情说明
        /// </summary>
        [JsonProperty("memo")]
        [XmlElement("memo")]
        public string Memo { get; set; }

        /// <summary>
        /// 商户id
        /// </summary>
        [JsonProperty("merchant_uniq_id")]
        [XmlElement("merchant_uniq_id")]
        public string MerchantUniqId { get; set; }

        /// <summary>
        /// 是否可叠加
        /// </summary>
        [JsonProperty("multi_use_flag")]
        [XmlElement("multi_use_flag")]
        public string MultiUseFlag { get; set; }

        /// <summary>
        /// 红包名称
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 是否可退款标识
        /// </summary>
        [JsonProperty("refund_flag")]
        [XmlElement("refund_flag")]
        public string RefundFlag { get; set; }

        /// <summary>
        /// 红包状态信息
        /// </summary>
        [JsonProperty("status")]
        [XmlElement("status")]
        public string Status { get; set; }

        /// <summary>
        /// 红包模板编号
        /// </summary>
        [JsonProperty("template_no")]
        [XmlElement("template_no")]
        public string TemplateNo { get; set; }

        /// <summary>
        /// 用户openid
        /// </summary>
        [JsonProperty("user_id")]
        [XmlElement("user_id")]
        public string UserId { get; set; }
    }
}
