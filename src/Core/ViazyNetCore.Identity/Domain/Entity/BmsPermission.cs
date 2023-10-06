using FreeSql.DataAnnotations;

namespace ViazyNetCore.Authorization
{
    /// <summary>
    /// 表示一个角色权限。
    /// </summary>
    [Index("uk_BmsPermission_PermissionId_TenantId", "PermissionId ASC,TenantId ASC", IsUnique = true)]
    public partial class BmsPermission : Entity, ITenant
    {
        /// <summary>
        /// 设置或获取一个值，表示权限ID。
        /// </summary>
        public string PermissionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        [Column(MapType = typeof(int))]
        public PermissionType Type { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示处理程序。
        /// </summary>
        public string Handler { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long TenantId { get; set; }

    }

    public enum PermissionType
    {
        Menus = 1,
        Data = 2
    }
}
