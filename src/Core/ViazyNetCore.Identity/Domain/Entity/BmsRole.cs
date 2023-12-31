using FreeSql.DataAnnotations;

namespace ViazyNetCore.Authorization
{
    /// <summary>
    /// 表示一个角色。
    /// </summary>
    [Index("uk_BmsRole_Name", "Name Asc,TenantId Asc", IsUnique = true)]
    public partial class BmsRole : EntityUpdate, ITenant
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
        public string ExtraData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long TenantId { get; set; }
    }
}
