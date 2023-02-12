using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Authorization.Model
{
    /// <summary>
    /// 表示一个角色页面。
    /// </summary>
    public partial class BmsRolePage : EntityBase
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

    }
}
