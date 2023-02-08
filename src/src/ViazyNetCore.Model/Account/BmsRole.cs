using System;
using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个平台角色。
    /// </summary>
    public partial class BmsRole : EntityBase
    {
        /// <summary>
        /// 设置或获取一个值，表示编号。
        /// </summary>
        //[CharLength(20), Required]
        //public new string Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示名称。
        /// </summary>
        [MaxLength(50), Required]
        public string Name { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（0禁用，1启用，-1删除）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示修改时间。
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string ExtraData { get; set; }

    }
}
