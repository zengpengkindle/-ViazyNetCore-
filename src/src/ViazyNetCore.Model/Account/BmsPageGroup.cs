using System;
using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个平台页面分组。
    /// </summary>
    public partial class BmsPageGroup : EntityBase
    {
        /// <summary>
        /// 设置或获取一个值，表示上级编号。
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示分组标题。
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示分组图标。
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示分组排序。
        /// </summary>
        public int Sort { get; set; }

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

    }
}
