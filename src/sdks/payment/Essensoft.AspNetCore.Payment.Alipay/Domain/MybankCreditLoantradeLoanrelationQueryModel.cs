using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// MybankCreditLoantradeLoanrelationQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class MybankCreditLoantradeLoanrelationQueryModel : AlipayObject
    {
        /// <summary>
        /// 工商注册号或者身份证号码
        /// </summary>
        [JsonProperty("cert_no")]
        [XmlElement("cert_no")]
        public string CertNo { get; set; }

        /// <summary>
        /// 当客户为公司时，certtype是全国组织机构代码证书。当客户为个人时，是居民身份证
        /// </summary>
        [JsonProperty("cert_type")]
        [XmlElement("cert_type")]
        public string CertType { get; set; }

        /// <summary>
        /// 预留的扩展字段
        /// </summary>
        [JsonProperty("ext_params")]
        [XmlElement("ext_params")]
        public string ExtParams { get; set; }

        /// <summary>
        /// 政策码
        /// </summary>
        [JsonProperty("loan_policy_code")]
        [XmlElement("loan_policy_code")]
        public string LoanPolicyCode { get; set; }

        /// <summary>
        /// 当客户是公司时，entityname是公司名全称；当客户是个人时，entityname是姓名
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 产品码
        /// </summary>
        [JsonProperty("product_code")]
        [XmlElement("product_code")]
        public string ProductCode { get; set; }
    }
}
