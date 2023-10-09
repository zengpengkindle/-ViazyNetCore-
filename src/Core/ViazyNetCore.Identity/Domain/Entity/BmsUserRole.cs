using FreeSql.DataAnnotations;

namespace ViazyNetCore.Authorization
{
    /// <summary>
    /// 表示一个BmsUserRole。
    /// </summary>
    [Index("uk_UserId_RoleId", "RoleId Asc,UserId Asc", IsUnique = true)]
    public partial class BmsUserRole : Entity, ITenant
    {
        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long TenantId { get; set; }
    }
}
