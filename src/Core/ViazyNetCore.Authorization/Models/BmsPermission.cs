namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 表示一个角色权限。
    /// </summary>
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
}
