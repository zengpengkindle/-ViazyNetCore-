using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEbppIndustryOrderCreateModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEbppIndustryOrderCreateModel : AlipayObject
    {
        /// <summary>
        /// 能力码是由支付宝分配的标识code
        /// </summary>
        [JsonProperty("ability_code")]
        [XmlElement("ability_code")]
        public string AbilityCode { get; set; }

        /// <summary>
        /// 账单的账期，例如201703表示2017年3月的账单。
        /// </summary>
        [JsonProperty("bill_date")]
        [XmlElement("bill_date")]
        public string BillDate { get; set; }

        /// <summary>
        /// 业务账户号，例如水费单号，手机号，电费号，信用卡卡号。没有唯一性要求。
        /// </summary>
        [JsonProperty("bill_key")]
        [XmlElement("bill_key")]
        public string BillKey { get; set; }

        /// <summary>
        /// 用户创建订单时涉及到的业务总金额。单位为：RMB Yuan。取值范围为[0.01，100000000.00)，精确到小数点后两位。
        /// </summary>
        [JsonProperty("biz_amount")]
        [XmlElement("biz_amount")]
        public string BizAmount { get; set; }

        /// <summary>
        /// 机构简称例如杭州电力HZELECTRIC
        /// </summary>
        [JsonProperty("biz_inst_short_name")]
        [XmlElement("biz_inst_short_name")]
        public string BizInstShortName { get; set; }

        /// <summary>
        /// 业务类型，缴费业务为JF，还款业务HK，公服行业为IND。
        /// </summary>
        [JsonProperty("biz_type")]
        [XmlElement("biz_type")]
        public string BizType { get; set; }

        /// <summary>
        /// 城市编码，国标码
        /// </summary>
        [JsonProperty("city_code")]
        [XmlElement("city_code")]
        public string CityCode { get; set; }

        /// <summary>
        /// 扩展属性，json串(key-value对)
        /// </summary>
        [JsonProperty("extend_field")]
        [XmlElement("extend_field")]
        public string ExtendField { get; set; }

        /// <summary>
        /// 滞纳金额，单位：元
        /// </summary>
        [JsonProperty("fine_amount")]
        [XmlElement("fine_amount")]
        public string FineAmount { get; set; }

        /// <summary>
        /// 身份标识号，例如身份证号、纳税人识别号等。
        /// </summary>
        [JsonProperty("identity_no")]
        [XmlElement("identity_no")]
        public string IdentityNo { get; set; }

        /// <summary>
        /// 用户的手机号
        /// </summary>
        [JsonProperty("mobile")]
        [XmlElement("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// ISV流水号，用于控制幂等，须确保全局唯一
        /// </summary>
        [JsonProperty("out_order_no")]
        [XmlElement("out_order_no")]
        public string OutOrderNo { get; set; }

        /// <summary>
        /// 拥有该订单的用户姓名
        /// </summary>
        [JsonProperty("owner_name")]
        [XmlElement("owner_name")]
        public string OwnerName { get; set; }

        /// <summary>
        /// 省份编码，国标码
        /// </summary>
        [JsonProperty("province_code")]
        [XmlElement("province_code")]
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 子业务类型，水费为WATER，燃气费为GAS，社保为SI，公积金为HF，社保+公积金为SIHF
        /// </summary>
        [JsonProperty("sub_biz_type")]
        [XmlElement("sub_biz_type")]
        public string SubBizType { get; set; }
    }
}
