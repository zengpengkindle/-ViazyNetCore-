namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个商品子订单。
    /// </summary>
    [Table(Name = "ShopMall.ProductTradeOrder")]
    public partial class ProductTradeOrder : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示是否有外部关联。
        /// </summary>
        public bool HasOuter { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示外部关联类型，多类型使用,隔开。
        /// </summary>
        public string OuterType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示订单号。
        /// </summary>
        public string ProductTradeId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品编号。
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示品牌编号。
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示品牌名称。
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品图片。
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SkuId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品外部编号。
        /// </summary>
        public string OuterId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品外部Sku编码。
        /// </summary>
        public string OuterSkuId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品规格名称组合文本,如"颜色:红色 尺码:32"。
        /// </summary>
        public string SkuText { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品数量。
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示成本价。
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 退换货类型 1.可以退换货，2.只能退货 3.只能换货 4.不能退换货。
        /// </summary>
        public RefundType RefundType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品售价。
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示订单金额。
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

    }
}
