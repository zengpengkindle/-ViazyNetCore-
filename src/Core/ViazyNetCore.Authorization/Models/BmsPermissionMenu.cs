using ViazyNetCore.Authorization.Model;

namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BmsPermissionMenu : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示名称。
        /// </summary>
        public string PermissionItemKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MenuId { get; set; }

    }
}