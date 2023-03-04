using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AntMerchantExpandIndirectCreateModel Data Structure.
    /// </summary>
    [Serializable]
    public class AntMerchantExpandIndirectCreateModel : AlipayObject
    {
        /// <summary>
        /// 商户地址信息
        /// </summary>
        [JsonProperty("address_info")]
        [XmlArray("address_info")]
        [XmlArrayItem("address_info")]
        public List<AddressInfo> AddressInfo { get; set; }

        /// <summary>
        /// 商户简称
        /// </summary>
        [JsonProperty("alias_name")]
        [XmlElement("alias_name")]
        public string AliasName { get; set; }

        /// <summary>
        /// 商户对应银行所开立的结算卡信息
        /// </summary>
        [JsonProperty("bankcard_info")]
        [XmlArray("bankcard_info")]
        [XmlArrayItem("bank_card_info")]
        public List<BankCardInfo> BankcardInfo { get; set; }

        /// <summary>
        /// 商户证件编号（企业或者个体工商户提供营业执照，事业单位提供事证号）
        /// </summary>
        [JsonProperty("business_license")]
        [XmlElement("business_license")]
        public string BusinessLicense { get; set; }

        /// <summary>
        /// 商户证件类型，取值范围：NATIONAL_LEGAL：营业执照；NATIONAL_LEGAL_MERGE:营业执照(多证合一)；INST_RGST_CTF：事业单位法人证书
        /// </summary>
        [JsonProperty("business_license_type")]
        [XmlElement("business_license_type")]
        public string BusinessLicenseType { get; set; }

        /// <summary>
        /// 商户经营类目，参考文档：https://doc.open.alipay.com/doc2/detail?&docType=1&articleId=105444，非银联/网联进件时必传
        /// </summary>
        [JsonProperty("category_id")]
        [XmlElement("category_id")]
        public string CategoryId { get; set; }

        /// <summary>
        /// 商户联系人信息
        /// </summary>
        [JsonProperty("contact_info")]
        [XmlArray("contact_info")]
        [XmlArrayItem("contact_info")]
        public List<ContactInfo> ContactInfo { get; set; }

        /// <summary>
        /// 商户编号，由机构定义，需要保证在机构下唯一
        /// </summary>
        [JsonProperty("external_id")]
        [XmlElement("external_id")]
        public string ExternalId { get; set; }

        /// <summary>
        /// 商户的支付宝账号
        /// </summary>
        [JsonProperty("logon_id")]
        [XmlArray("logon_id")]
        [XmlArrayItem("string")]
        public List<string> LogonId { get; set; }

        /// <summary>
        /// 标准商户类别码，例如5976表示“专业销售-药品医疗-康复和身体辅助用品”，银联/网联进件时必传
        /// </summary>
        [JsonProperty("mcc")]
        [XmlElement("mcc")]
        public string Mcc { get; set; }

        /// <summary>
        /// 商户备注，可填写额外信息。分支机构进件，需要按照要求填写“分支机构码”，方便进行入驻管控，分支机构码由支付宝指定编码值，具体编码值可联系对口BD获取。填写分支机构码的时候用“##”标识符括起来，放在整条备注信息的开头处。示例：若进件分支机构为吉林省，由于对应分支机构编码值为220000，那么进件的时候应填写备注信息为：##220000##其他备注信息。
        /// </summary>
        [JsonProperty("memo")]
        [XmlElement("memo")]
        public string Memo { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 收单机构(例如银行）的标识，填写该机构在支付宝的pid。银联/网联进件时必传。
        /// </summary>
        [JsonProperty("org_pid")]
        [XmlElement("org_pid")]
        public string OrgPid { get; set; }

        /// <summary>
        /// 商户的支付二维码中信息，用于营销活动
        /// </summary>
        [JsonProperty("pay_code_info")]
        [XmlArray("pay_code_info")]
        [XmlArrayItem("string")]
        public List<string> PayCodeInfo { get; set; }

        /// <summary>
        /// 商户客服电话
        /// </summary>
        [JsonProperty("service_phone")]
        [XmlElement("service_phone")]
        public string ServicePhone { get; set; }

        /// <summary>
        /// 间连受理商户的推荐组织。如果是银行自有商户入驻，则推荐组织为银行，如果是ISV推广的商户，那么商户推荐组织为ISV，如果是第三方支付机构的自有商户，则推荐组织为第三方支付机构。
        /// </summary>
        [JsonProperty("source")]
        [XmlElement("source")]
        public string Source { get; set; }
    }
}
