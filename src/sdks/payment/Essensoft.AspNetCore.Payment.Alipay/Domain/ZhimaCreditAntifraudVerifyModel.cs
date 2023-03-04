using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// ZhimaCreditAntifraudVerifyModel Data Structure.
    /// </summary>
    [Serializable]
    public class ZhimaCreditAntifraudVerifyModel : AlipayObject
    {
        /// <summary>
        /// 地址信息。省+市+区/县+详细地址，长度不超过256，不要包含特殊字符，如","，"\"，"|"，"&"，"^"
        /// </summary>
        [JsonProperty("address")]
        [XmlElement("address")]
        public string Address { get; set; }

        /// <summary>
        /// 仅支持大陆银行卡验证，包括信用卡、借记卡等实体卡。
        /// </summary>
        [JsonProperty("bank_card")]
        [XmlElement("bank_card")]
        public string BankCard { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        [JsonProperty("cert_no")]
        [XmlElement("cert_no")]
        public string CertNo { get; set; }

        /// <summary>
        /// 证件类型。IDENTITY_CARD标识为身份证，目前仅支持身份证类型
        /// </summary>
        [JsonProperty("cert_type")]
        [XmlElement("cert_type")]
        public string CertType { get; set; }

        /// <summary>
        /// 电子邮箱。合法email，字母小写，特殊符号以半角形式出现
        /// </summary>
        [JsonProperty("email")]
        [XmlElement("email")]
        public string Email { get; set; }

        /// <summary>
        /// 国际移动设备标志。15位长度数字
        /// </summary>
        [JsonProperty("imei")]
        [XmlElement("imei")]
        public string Imei { get; set; }

        /// <summary>
        /// ip地址。以"."分割的四段Ip，如 x.x.x.x，x为[0,255]之间的整数
        /// </summary>
        [JsonProperty("ip")]
        [XmlElement("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// 物理地址。支持格式如下：xx:xx:xx:xx:xx:xx，xx-xx-xx-xx-xx-xx，xxxxxxxxxxxx，x取值范围[0,9]之间的整数及A，B，C，D，E，F
        /// </summary>
        [JsonProperty("mac")]
        [XmlElement("mac")]
        public string Mac { get; set; }

        /// <summary>
        /// 手机号码。中国大陆合法手机号，长度11位，不含国家代码
        /// </summary>
        [JsonProperty("mobile")]
        [XmlElement("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// 姓名，长度不超过64，姓名中不含",","/u0001"，"|","&","^","\\"
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 产品码，标记商户接入的具体产品；直接使用每个接口入参中示例值即可
        /// </summary>
        [JsonProperty("product_code")]
        [XmlElement("product_code")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 商户请求的唯一标志，长度64位以内字符串，仅限字母数字下划线组合。该标识作为业务调用的唯一标识，商户要保证其业务唯一性，使用相同transaction_id的查询，芝麻在一段时间内（一般为1天）返回首次查询结果，超过有效期的查询即为无效并返回异常，有效期内的重复查询不重新计费
        /// </summary>
        [JsonProperty("transaction_id")]
        [XmlElement("transaction_id")]
        public string TransactionId { get; set; }

        /// <summary>
        /// wifi的物理地址。支持格式如下：xx:xx:xx:xx:xx:xx，xx-xx-xx-xx-xx-xx，xxxxxxxxxxxx，x取值范围[0,9]之间的整数及A，B，C，D，E，F
        /// </summary>
        [JsonProperty("wifimac")]
        [XmlElement("wifimac")]
        public string Wifimac { get; set; }
    }
}
