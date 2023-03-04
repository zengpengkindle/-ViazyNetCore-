namespace ViazyNetCore.Model
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductTradeSet : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示用户编号。
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示子订单数量。
        /// </summary>
        public int TradeCount { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示业务类型。
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示总运费。
        /// </summary>
        public decimal TotalFreight { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品总金额。
        /// </summary>
        public decimal ProductMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示总金额。
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品支付方式（-1未付款，0微信支付，1支付宝，2余额，3虚拟货币）。
        /// </summary>
        public PayMode PayMode { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示订单应付款。
        /// </summary>
        public decimal PayMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示实付金额。
        /// </summary>
        public decimal? RealPayMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示实付运费。
        /// </summary>
        public decimal? RealPayFreight { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示支付时间。
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示订单集合状态（-1已关闭，0未付款，1已付款）。
        /// </summary>
        public TradeSetStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态变更时间。
        /// </summary>
        public DateTime StatusChangedTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Exdata { get; set; }

    }
}
