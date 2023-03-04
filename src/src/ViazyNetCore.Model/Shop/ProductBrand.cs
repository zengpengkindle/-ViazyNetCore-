namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个商品品牌。
    /// </summary>
    public partial class ProductBrand : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示是否在前台隐藏。
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示图片。
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（-1删除,0禁用，1启用）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示排序。
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

    }
}
