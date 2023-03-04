using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AiOcrTableRow Data Structure.
    /// </summary>
    [Serializable]
    public class AiOcrTableRow : AlipayObject
    {
        /// <summary>
        /// table一行的内容
        /// </summary>
        [JsonProperty("row")]
        [XmlArray("row")]
        [XmlArrayItem("ai_ocr_table_context")]
        public List<AiOcrTableContext> Row { get; set; }
    }
}
