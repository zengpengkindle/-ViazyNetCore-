namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 表示一个BmsUserRole。
    /// </summary>
    public partial class BmsUserRole : EntityBase<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long RoleId { get; set; }

    }
}
