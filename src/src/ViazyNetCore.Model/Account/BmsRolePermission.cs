using System;
using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个角色权限。
    /// </summary>
    public partial class BmsRolePermission : EntityBase
    {
        /// <summary>
        /// 设置或获取一个值，表示角色编号。
        /// </summary>
        [Required]
        public long RoleId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示权限项编号。
        /// </summary>
        [Required]
        public string ItemId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
