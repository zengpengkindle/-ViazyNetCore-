using Newtonsoft.Json;

namespace ViazyNetCore.Formatter.Response
{
    public class ResponseWrapperOptions : OptionBase
    {
        public bool LogRequestDataOnException { get; set; } = true;

        public bool ShouldLogRequestData { get; set; } = true;

        /// <summary>
        /// 必须生产环境且开启，才会开启加密
        /// </summary>
        public bool EnableCipher { get; set; } = false;
    }
}
