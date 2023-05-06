namespace ViazyNetCore.Dtos
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

        /// <summary>
        /// 所属部门Ids
        /// </summary>
        public List<long> OrgIds { get; set; }

        /// <summary>
        /// 主属部门Id
        /// </summary>
        public long OrgId { get; set; }
    }
}