using FreeSql.DataAnnotations;

namespace ViazyNetCore.Authorization
{
    /// <summary>
    /// 表示一个功能权限。
    /// </summary>
    [Index("uk_BmsOwnerPermission_PermissionItemKey_OwnerId_OwnerType"
        , fields: "PermissionItemKey Asc,OwnerId Asc,OwnerType Asc"
        , isUnique: true)]
    public partial class BmsOwnerPermission : Entity<string>, ITenant
    {
        /// <summary>
        /// 设置或获取一个值，表示名称。
        /// </summary>
        public string PermissionItemKey { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（-1删除，0禁用，1启用）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示被授权对象类型（1用户,11角色）。
        /// </summary>
        public OwnerType OwnerType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLock { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long TenantId { get; set; }

    }
}
