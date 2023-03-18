namespace ViazyNetCore.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table(Name = "ShopMall.RefundTradeOrder")]
    public partial class RefundTradeOrder : EntityBase<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string RefundTradeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TradeOrderId { get; set; }

        public string ProductName { get; set; }

        public string SkuText { get; set; }

        public int Num { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RefundStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 PUPAWAY:退货入库;REDELIVERY:重新发货;RECLAIM-REDELIVERY:不要求归还并重新发货; REFUND:退款; COMPENSATION:不退货并赔偿。
        /// </summary>
        public HandlingType? HandlingType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? HandlingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; }

    }
}
