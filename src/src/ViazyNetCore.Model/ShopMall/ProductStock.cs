namespace ViazyNetCore.Model
{
    /// <summary>
    /// 商品库存记录
    /// </summary>
    [Table(Name = "ShopMall.ProductStock")]
    public partial class ProductStock : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示商品编号。
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品Sku编号。（不为空时表示对应商品存在sku，商品将存在多条相同productId的productStock记录。商品总库存为所有productStock记录总和）
        /// </summary>
        public string ProductSkuId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示在库数量。
        /// </summary>
        public int InStock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示锁定数量（待付款商品）。
        /// </summary>
        public int Lock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示待发货数。(废弃)
        /// </summary>
        //public int UnDeliver { get; set; }

        
        /// <summary>
        /// 设置或获取一个值，表示出库数。
        /// </summary>
        public int OutStock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示销量。
        /// </summary>
        public int SellNum { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示退货数。
        /// </summary>
        public int Refund { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示换货数。
        /// </summary>
        public int Exchange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
