namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个商品分类。
    /// </summary>
    [Table(Name = "ShopMall.ProductCat")]
    public partial class ProductCat : EntityBase<string>
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
        /// 设置或获取一个值，表示上级编号。
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示是否父级。
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类目路径。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示图片。
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示排序。
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（-1删除,0禁用，1启用）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

        [Column(CanUpdate = false)]
        public DateTime CreateTime { get; set; }
    }
}
