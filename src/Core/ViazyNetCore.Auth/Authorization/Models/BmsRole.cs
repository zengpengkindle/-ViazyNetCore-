using ViazyNetCore.Authorization.Model;

namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 表示一个角色。
    /// </summary>
    public partial class BmsRole : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（-1删除，0禁用，1启用）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }
        public string ExtraData { get; internal set; }
    }
}
