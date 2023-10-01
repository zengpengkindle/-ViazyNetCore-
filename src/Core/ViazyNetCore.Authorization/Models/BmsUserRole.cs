namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 表示一个BmsUserRole。
    /// </summary>
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
