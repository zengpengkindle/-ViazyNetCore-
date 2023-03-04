using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// MybankCreditSceneprodLoanModifyModel Data Structure.
    /// </summary>
    [Serializable]
    public class MybankCreditSceneprodLoanModifyModel : AlipayObject
    {
        /// <summary>
        /// 业务订单扩展字段，根据机构不同填入的参数会有区别
        /// </summary>
        [JsonProperty("ext_param")]
        [XmlElement("ext_param")]
        public string ExtParam { get; set; }

        /// <summary>
        /// 扩展字段实体，规范了通用的扩展字段内的信息，便于调用方使用，与ext_param作用相同。与ext_param有同名字段冲突时以ext_param_info内容为准。
        /// </summary>
        [JsonProperty("ext_param_info")]
        [XmlElement("ext_param_info")]
        public SceneExtParam ExtParamInfo { get; set; }

        /// <summary>
        /// 出资机构在网商的角色号（ipRoleId）, 不需要更改可以不填
        /// </summary>
        [JsonProperty("finance_inst_iprole_id")]
        [XmlElement("finance_inst_iprole_id")]
        public string FinanceInstIproleId { get; set; }

        /// <summary>
        /// 贷款金额，单位：分 。如果不需要更改可以不传
        /// </summary>
        [JsonProperty("loan_amt")]
        [XmlElement("loan_amt")]
        public string LoanAmt { get; set; }

        /// <summary>
        /// 贷款期数，单位：月，如果不需要更改可以不传
        /// </summary>
        [JsonProperty("loan_period")]
        [XmlElement("loan_period")]
        public string LoanPeriod { get; set; }

        /// <summary>
        /// 机构编码，机构接入场景金融平台时分配，固定值
        /// </summary>
        [JsonProperty("org_code")]
        [XmlElement("org_code")]
        public string OrgCode { get; set; }

        /// <summary>
        /// 外部平台订单号,64个字符以内、只能包含字母、数字、下划线；需保证在外部平台端不重复
        /// </summary>
        [JsonProperty("out_order_no")]
        [XmlElement("out_order_no")]
        public string OutOrderNo { get; set; }

        /// <summary>
        /// 产品编码，场景金融平台给机构提供的产品编码
        /// </summary>
        [JsonProperty("product_code")]
        [XmlElement("product_code")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 外部站点，比如：ALIPAY：支付宝站点，MYBANK：银行会员，B2B_CN：B2B中文站，B2B_EN：B2B国际站，TAOBAO：淘宝
        /// </summary>
        [JsonProperty("site")]
        [XmlElement("site")]
        public string Site { get; set; }

        /// <summary>
        /// 外部站点的userid，比如支付宝userid
        /// </summary>
        [JsonProperty("site_user_id")]
        [XmlElement("site_user_id")]
        public string SiteUserId { get; set; }

        /// <summary>
        /// 要更改的订单状态
        /// </summary>
        [JsonProperty("status")]
        [XmlElement("status")]
        public string Status { get; set; }
    }
}
