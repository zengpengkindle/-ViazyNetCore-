namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个商品SKU。
    /// </summary>
    [Table(Name = "ShopMall.ProductSku")]
    public partial class ProductSku : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示商品编号。
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示外部SKU编码。
        /// </summary>
        public string OuterSkuId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 规格类目 k_s 为 s1 的对应规格值 id
        /// </summary>
        public string S1 { get; set; }

        /// <summary>
        /// 规格s1名称 如：颜色
        /// </summary>
        public string Key1 { get; set; }

        /// <summary>
        /// 规格s1名称 如：颜色
        /// </summary>
        public string Name1 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 规格类目 k_s 为 s2 的对应规格值 id 为0表示不存在该规格
        /// </summary>
        public string S2 { get; set; }

        /// <summary>
        /// 规格s2名称 如：颜色
        /// </summary>
        public string Key2 { get; set; }

        /// <summary>
        /// 规格s2值 如：红色
        /// </summary>
        public string Name2 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 规格类目 k_s 为 s3 的对应规格值 id 为0表示不存在该规格
        /// </summary>
        public string S3 { get; set; }

        /// <summary>
        /// 规格s3名称 如：颜色
        /// </summary>
        public string Key3 { get; set; }

        /// <summary>
        /// 规格s3名称 如：颜色
        /// </summary>
        public string Name3 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示成本。
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示售价。
        /// </summary>
        public decimal Price { get; set; }

        
        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

    }
}
