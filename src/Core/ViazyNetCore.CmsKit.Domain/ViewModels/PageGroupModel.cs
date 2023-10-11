using System.ComponentModel.DataAnnotations;
using ViazyNetCore.CmsKit;

namespace ViazyNetCore.ViewModels
{
    /// <summary>
    /// 表示页面分组的模型。
    /// </summary>
    public class PageGroupModel
    {
        /// <summary>
        /// 设置或获取一个值，表示编号。
        /// </summary>
        [MaxLength(50)]
        public long Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示上级编号。
        /// </summary>
        [MaxLength(50)]
        public long ParentId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示分组标题。
        /// </summary>
        [MaxLength(100), Required]
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示分组图标。
        /// </summary>
        [MaxLength(100)]
        public string Icon { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示分组排序。
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（0禁用，1启用，-1删除）。
        /// </summary>
        public ComStatus Status { get; set; }

    }
}
