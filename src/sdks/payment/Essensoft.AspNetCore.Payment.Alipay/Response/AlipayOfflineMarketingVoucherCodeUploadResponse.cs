using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Response
{
    /// <summary>
    /// AlipayOfflineMarketingVoucherCodeUploadResponse.
    /// </summary>
    public class AlipayOfflineMarketingVoucherCodeUploadResponse : AlipayResponse
    {
        /// <summary>
        /// 码库id
        /// </summary>
        [JsonProperty("code_inventory_id")]
        [XmlElement("code_inventory_id")]
        public string CodeInventoryId { get; set; }
    }
}
