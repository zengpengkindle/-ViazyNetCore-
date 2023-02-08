namespace ViazyNetCore.Domain
{
    /// <summary>
    /// 表示一个角色模型。
    /// </summary>
    public class RoleModel: RoleSimpleModel
    {
        /// <summary>
        /// 设置或获取一个值，表示状态（0禁用，1启用，-1删除）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string? ExtraData { get; set; }
    }
}