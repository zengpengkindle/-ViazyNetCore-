using Newtonsoft.Json;

namespace ViazyNetCore
{
    public class ApiResponse
    {
        public string Version { get; set; }

        [JsonProperty(PropertyName = "code")]
        public int StatusCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Message { get; set; }

        /// <summary>
        /// 堆栈信息,仅debug模式可见
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? StackTrace { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsError { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "error")]
        public object ResponseException { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "data")]
        public WrapperData Data { get; set; }

        [JsonConstructor]
        public ApiResponse(string message, object? result = null, int statusCode = 200, string apiVersion = "1.0.0.0")
        {
            StatusCode = statusCode;
            Message = message;
            Data = new WrapperData()
            {
                Success = true,
                Message = "",
                Result = result
            };
            Version = apiVersion;
        }

        public ApiResponse() { }
    }

    public class WrapperData
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "result")]
        public object? Result { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "err_code")]
        public int ErrorCode { get; set; } = 0;
    }
}