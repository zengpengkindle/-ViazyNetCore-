using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Authorization.Model
{
    /// <summary>
    /// 表示一个平台页面。
    /// </summary>
    public partial class BmsPage : EntityBase
    {
        /// <summary>
        /// 设置或获取一个值，表示页面分组编号。
        /// </summary>
        [MaxLength(20), Required]
        public long GroupId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示页面标题。
        /// </summary>
        [MaxLength(100), Required]
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示页面图标。
        /// </summary>
        [MaxLength(100)]
        public string Icon { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示页面路径。
        /// </summary>
        [MaxLength(100), Required]
        public string Url { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示页面排序。
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
