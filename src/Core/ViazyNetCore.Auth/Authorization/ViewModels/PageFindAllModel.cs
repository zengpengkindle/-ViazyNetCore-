using System;

namespace ViazyNetCore.ViewModels
{
    /// <summary>
    /// 表示一个平台页面的查询模型。
    /// </summary>
    public class PageFindAllModel: PageModel
    {
        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示修改时间。
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}