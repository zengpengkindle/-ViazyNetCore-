using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Response
{
    /// <summary>
    /// KoubeiMarketingDataActivityReportQueryResponse.
    /// </summary>
    public class KoubeiMarketingDataActivityReportQueryResponse : AlipayResponse
    {
        /// <summary>
        /// 报表
        /// </summary>
        [JsonProperty("report_data")]
        [XmlElement("report_data")]
        public string ReportData { get; set; }
    }
}
