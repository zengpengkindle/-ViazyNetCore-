using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 表示一个角色页面。
    /// </summary>
    public partial class BmsRolePage : Entity, ITenant
    {
        /// <summary>
        /// 设置或获取一个值，表示角色编号。
        /// </summary>
        [Required]
        public long RoleId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示页面编号。
        /// </summary>
        [Required]
        public long PageId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long TenantId { get; set; }

    }
}
