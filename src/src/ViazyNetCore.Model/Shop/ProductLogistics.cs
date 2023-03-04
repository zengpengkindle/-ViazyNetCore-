namespace ViazyNetCore.Model
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductLogistics : EntityBase<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public string TradeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExpressNo { get; set; }

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
        /// 
        /// </summary>
        public string LogisticsType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LogisticsId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LogisticsCompany { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流发货运费 。
        /// </summary>
        public decimal LogisticsFee { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示快递公司代收货款费率，如货值的2%-5%，一般月结。
        /// </summary>
        public decimal AgencyFee { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示实际支付给物流公司的金额。
        /// </summary>
        public decimal DeliveryAmount { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流状态。
        /// </summary>
        public int LogisticsStatus { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示发货状态。
        /// </summary>
        public DateTime LogisticsCreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流更新时间。
        /// </summary>
        public DateTime LogisticsUpdateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流结算时间。
        /// </summary>
        public DateTime? LogisticsSettlementTime { get; set; }
    }
}
