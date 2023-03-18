namespace ViazyNetCore.Model
{
    /// <summary>
    /// 退换货处理记录
    /// </summary>
    [Table(Name = "ShopMall.RefunTradeLog")]
    public partial class RefunTradeLog : EntityBase<string>
    {
        /// <summary>
        /// 非主楼层时，记录对应的主楼层Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RefundTradeId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示标题（1申请退货；2申请换货；3拒绝退货；4拒绝换货；5等待收取退货；6等待处理；7处理完毕）。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示0卖家信息 1买家信息。
        /// </summary>
        public RefunTradeLogType Type { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示说明。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示附加信息-物流单号（买家退回商品时物流单号）。
        /// </summary>
        public string LogisticsId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示附加信息-退款单所处状态。
        /// </summary>
        public RefundStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否为主楼层
        /// </summary>
        public bool IsParent { get; set; }

    }
}
