using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayPcreditLoanRepayplanQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayPcreditLoanRepayplanQueryModel : AlipayObject
    {
        /// <summary>
        /// 贷款申请单号，借呗客户申请贷款时系统生成的全局唯一业务流水号
        /// </summary>
        [JsonProperty("loan_apply_no")]
        [XmlElement("loan_apply_no")]
        public string LoanApplyNo { get; set; }
    }
}
