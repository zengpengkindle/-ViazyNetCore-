using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayDataDataserviceYuebaoassetDetailSendModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayDataDataserviceYuebaoassetDetailSendModel : AlipayObject
    {
        /// <summary>
        /// 资产负债报表数据列表
        /// </summary>
        [JsonProperty("alm_report_data")]
        [XmlArray("alm_report_data")]
        [XmlArrayItem("alm_report_data")]
        public List<AlmReportData> AlmReportData { get; set; }
    }
}
