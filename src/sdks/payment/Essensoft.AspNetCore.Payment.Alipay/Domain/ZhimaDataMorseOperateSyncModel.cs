using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// ZhimaDataMorseOperateSyncModel Data Structure.
    /// </summary>
    [Serializable]
    public class ZhimaDataMorseOperateSyncModel : AlipayObject
    {
        /// <summary>
        /// 外部商户发送给域内系统的信息
        /// </summary>
        [JsonProperty("data")]
        [XmlElement("data")]
        public string Data { get; set; }

        /// <summary>
        /// 扩展字段，用于补充
        /// </summary>
        [JsonProperty("extent_info")]
        [XmlElement("extent_info")]
        public string ExtentInfo { get; set; }

        /// <summary>
        /// 多方安全计算分配给商户的唯一id，由管理员分配给商户的唯一识别号；
        /// </summary>
        [JsonProperty("mid")]
        [XmlElement("mid")]
        public string Mid { get; set; }

        /// <summary>
        /// 服务操作类型，商户同步信息到域内，类型枚举：revoke_dataset、public_dataset、approve_sercive；根据不同场景管理员分配；
        /// </summary>
        [JsonProperty("operate_type")]
        [XmlElement("operate_type")]
        public string OperateType { get; set; }
    }
}
