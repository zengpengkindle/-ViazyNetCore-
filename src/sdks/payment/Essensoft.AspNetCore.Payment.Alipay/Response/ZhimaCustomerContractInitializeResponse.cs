using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Response
{
    /// <summary>
    /// ZhimaCustomerContractInitializeResponse.
    /// </summary>
    public class ZhimaCustomerContractInitializeResponse : AlipayResponse
    {
        /// <summary>
        /// 电子合约号，后续的电子签名流程需要用到
        /// </summary>
        [JsonProperty("contract_no")]
        [XmlElement("contract_no")]
        public string ContractNo { get; set; }
    }
}
