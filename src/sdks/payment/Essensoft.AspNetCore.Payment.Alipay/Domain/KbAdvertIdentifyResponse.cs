using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// KbAdvertIdentifyResponse Data Structure.
    /// </summary>
    [Serializable]
    public class KbAdvertIdentifyResponse : AlipayObject
    {
        /// <summary>
        /// 根据benefit_type，确定ID含义  SINGLE_VOUCHER时，benefit_ids为券ID
        /// </summary>
        [JsonProperty("benefit_ids")]
        [XmlArray("benefit_ids")]
        [XmlArrayItem("string")]
        public List<string> BenefitIds { get; set; }

        /// <summary>
        /// 发放权益类型  SINGLE_VOUCHER：单券
        /// </summary>
        [JsonProperty("benefit_type")]
        [XmlElement("benefit_type")]
        public string BenefitType { get; set; }

        /// <summary>
        /// 返回码  success: 成功  invalid-arguments: 无效参数  retry-exception: 异常请重试  isv.user-already-get-voucher：用户已经领过该券，同时券状态为有效  isv.item_inventory_not_enough:优惠领光了  isv.item_not_in_this_shop_sales:不是该商家的优惠，不能领取  isv.voucher_activity_not_started:活动未开始  isv.voucher_activity_expired:活动已结束  isv.crowd_limit_not_match_error:暂无领取资格，详情请咨询商家  isv.member_crowd_limit_not_match_error:会员专属，请先注册会员
        /// </summary>
        [JsonProperty("code")]
        [XmlElement("code")]
        public string Code { get; set; }

        /// <summary>
        /// JSON格式数据，alipass_url为打开钱包的地址
        /// </summary>
        [JsonProperty("ext_info")]
        [XmlElement("ext_info")]
        public string ExtInfo { get; set; }

        /// <summary>
        /// 主键的值
        /// </summary>
        [JsonProperty("identify")]
        [XmlElement("identify")]
        public string Identify { get; set; }

        /// <summary>
        /// 主键类型
        /// </summary>
        [JsonProperty("identify_type")]
        [XmlElement("identify_type")]
        public string IdentifyType { get; set; }
    }
}
