namespace ViazyNetCore.Domain
{
    /// <summary>
    /// 表示一个用户的模型。
    /// </summary>
    public class UserModel : UserModelBase
    {
        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string? ExtraData { get; set; }
    }
}