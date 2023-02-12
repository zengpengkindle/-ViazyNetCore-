using ViazyNetCore.Authorization.Model;

namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 表示一个BmsUserRole。
    /// </summary>
    public partial class BmsUserRole : EntityBase<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RoleId { get; set; }

    }
}
