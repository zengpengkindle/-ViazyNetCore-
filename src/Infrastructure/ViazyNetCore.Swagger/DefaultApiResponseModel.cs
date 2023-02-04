using Newtonsoft.Json;

namespace ViazyNetCore.Swagger
{
    public class DefaultApiResponseModel<T>
    {
        [JsonProperty(PropertyName = "code")]
        public int StatusCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "data")]
        public Data<T> Data { get; set; }

        public DefaultApiResponseModel() { }
    }

    public class Data<T>
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "result")]
        public T Result { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "err_code")]
        public int ErrorCode { get; set; } = 0;
    }

    public class DefaultApiResponseModel
    {
        [JsonProperty(PropertyName = "code")]
        public int StatusCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "data")]
        public Data Data { get; set; }

        public DefaultApiResponseModel() { }
    }

    public class Data
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, PropertyName = "err_code")]
        public int ErrorCode { get; set; } = 0;
    }
}
