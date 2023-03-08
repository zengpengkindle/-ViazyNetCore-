namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个商品订单。
    /// </summary>
    [Table(Name = "ShopMall.ProductTrade")]
    public partial class ProductTrade : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示会员用户编号
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示会员用户名称
        /// </summary>
        public string MemberName { get; set; }

        public string ShopId { get; set; }

        public string ShopName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示所属支付编号。
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示是否有外部关联。
        /// </summary>
        public bool HasOuter { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示外部关联类型，多类型使用,隔开。
        /// </summary>
        public string OuterType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示业务类型。
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示总运费。
        /// </summary>
        public decimal TotalFreight { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示总商品金额。
        /// </summary>
        public decimal ProductMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示总消费金额。
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货地址所在的省份。
        /// </summary>
        public string ReceiverProvince { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货地址所在的城市。
        /// </summary>
        public string ReceiverCity { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货地址所在的区县。
        /// </summary>
        public string ReceiverDistrict { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示详细的收货地址。
        /// </summary>
        public string ReceiverDetail { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货人姓名。
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货人手机。
        /// </summary>
        public string ReceiverMobile { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流公司编号。
        /// </summary>
        public string LogisticsId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流单号。
        /// </summary>
        public string LogisticsCode { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流公司名称。
        /// </summary>
        public string LogisticsCompany { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示订单物流成本
        /// </summary>
        public decimal? LogisticsCost { get; set; }

        public PayMode PayMode { get; set; }

        /// <summary>
        ///  设置或获取一个值，表示会员备注
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 设置或获取一个值，表示下单时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示付款时间。
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示发货时间。
        /// </summary>
        public DateTime? ConsignTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示完成时间。
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示订单状态（-2待提货，-1未付款，0待发货，1待收货，2已成功，3已关闭）。
        /// </summary>
        public TradeStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示订单状态变更时间。
        /// </summary>
        public DateTime StatusChangedTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

    }
}
