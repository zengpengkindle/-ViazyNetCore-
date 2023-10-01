using System.Text.Json.Serialization;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示一个用户的单项查询模型。
    /// </summary>
    public class UserWithGoogleKeyDto : UserBaseDto
    {
        /// <summary>
        /// 设置或获取一个值，表示谷歌验证器密钥。
        /// </summary>
        [JsonIgnore]
        public string GoogleKey { get; set; }
    }
}