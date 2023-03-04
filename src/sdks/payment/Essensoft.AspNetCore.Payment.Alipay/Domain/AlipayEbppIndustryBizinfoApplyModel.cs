using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEbppIndustryBizinfoApplyModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEbppIndustryBizinfoApplyModel : AlipayObject
    {
        /// <summary>
        /// 业务能力码，标识业务场景
        /// </summary>
        [JsonProperty("ability_code")]
        [XmlElement("ability_code")]
        public string AbilityCode { get; set; }

        /// <summary>
        /// 业务账户号，例如水费单号，手机号，电费号，信用卡卡号。没有唯一性要求。
        /// </summary>
        [JsonProperty("bill_key")]
        [XmlElement("bill_key")]
        public string BillKey { get; set; }

        /// <summary>
        /// 业务机构简称
        /// </summary>
        [JsonProperty("biz_inst")]
        [XmlElement("biz_inst")]
        public string BizInst { get; set; }

        /// <summary>
        /// 业务类型，公服业务：IND
        /// </summary>
        [JsonProperty("biz_type")]
        [XmlElement("biz_type")]
        public string BizType { get; set; }

        /// <summary>
        /// 外部申请流水号，支持幂等
        /// </summary>
        [JsonProperty("out_apply_no")]
        [XmlElement("out_apply_no")]
        public string OutApplyNo { get; set; }

        /// <summary>
        /// 请求上下文，json格式
        /// </summary>
        [JsonProperty("request_context")]
        [XmlElement("request_context")]
        public string RequestContext { get; set; }
    }
}
