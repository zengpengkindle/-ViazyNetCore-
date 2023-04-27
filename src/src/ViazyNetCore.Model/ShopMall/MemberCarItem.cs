namespace ViazyNetCore.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table(Name = "ShopMall.MemberCarItem")]
    public partial class MemberCarItem : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示购物车Id(同MemberId)。
        /// </summary>
        public long CarId { get; set; }
        
        /// <summary>
        /// 设置或获取一个值，表示商品Id。
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示SkuId。
        /// </summary>
        public string SkuId { get; set; }

        /// <summary>
        /// 商品所属商店
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示下单后的ProductTradeOrder.Id。
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品名称。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示规格描述。
        /// </summary>
        public string SkuText { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示图片。
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品售价。
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示是否包邮。
        /// </summary>
        public bool IsFreeFreight { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示运费。
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示运费阶梯数，运费计算方式为：“订单商品数量/运费阶梯数”的值，如果存在小数数据则四舍五入取大值。
        /// </summary>
        public int FreightStep { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品消费货币1。
        /// </summary>
        public decimal? Credit1 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品消费货币2。
        /// </summary>
        public decimal? Credit2 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品消费货币3。
        /// </summary>
        public decimal? Credit3 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品消费货币4。
        /// </summary>
        public decimal? Credit4 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品消费货币5。
        /// </summary>
        public decimal? Credit5 { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购买数。
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购物车内商品状态（0已删除，1购物车内,2已结算）。
        /// </summary>
        public CarItemStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示加入时间。
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示修改时间。
        /// </summary>
        public DateTime? ChangedTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示下单时间。
        /// </summary>
        public DateTime? OrderTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示移除时间。
        /// </summary>
        public DateTime? RemoveTime { get; set; }

    }
}
