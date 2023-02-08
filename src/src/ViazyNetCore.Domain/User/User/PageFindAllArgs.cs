using System;
using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Domain
{
    /// <summary>
    /// 表示一个平台页面的查询参数。
    /// </summary>
    public class PageFindAllArgs : PaginationSort
    {
        /// <summary>
        /// 设置或获取一个值，表示页面分组编号。
        /// </summary>
        public long? GroupId { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示标题通配符。
        /// </summary>
        public string? TitleLike { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示状态，为空表示查询所有。
        /// </summary>
        public ComStatus? Status { get; set; }
    }
}