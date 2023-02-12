namespace ViazyNetCore.ViewModels
{
    /// <summary>
    /// 表示一个平台页面的模型。
    /// </summary>
    public class PageModel: PageSimpleModel
    {
        /// <summary>
        /// 设置或获取一个值，表示状态（0禁用，1启用，-1删除）。
        /// </summary>
        public ComStatus Status { get; set; }
    }
}