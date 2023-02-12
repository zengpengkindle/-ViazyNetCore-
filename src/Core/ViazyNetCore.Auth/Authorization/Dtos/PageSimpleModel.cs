using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示一个简单平台页面的模型。
    /// </summary>
    public class PageSimpleModel
    {
        /// <summary>
        /// 设置或获取一个值，表示编号。
        /// </summary>
        [MaxLength(50)]
        public long Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示页面分组编号。
        /// </summary>
        [MaxLength(50), Required]
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
    }
}
