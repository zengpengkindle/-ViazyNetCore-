namespace ViazyNetCore.Model
{
    /// <summary>
    /// 表示一个退换货记录
    /// </summary>
    [Table(Name = "ShopMall.RefundTrade")]
    public partial class RefundTrade : EntityBase<string>
    {
        /// <summary>
        /// 退货单号
        /// </summary>
        public string ReturnsNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TradeId { get; set; }
        
        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 退货收货人
        /// </summary>
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 退货人联系电话
        /// </summary>
        public string ConsigneeMobile { get; set; }

        /// <summary>
        /// 退货地址
        /// </summary>
        public string ConsigneeAddress { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示邮政编码。
        /// </summary>
        public string ConsigneeZip { get; set; }

        /// <summary>
        /// 物流公司编号
        /// </summary>
        public string LogisticsId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示产生的物流花费
        /// </summary>
        public decimal? LogisticsFee { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示全部退单,部分退单。
        /// </summary>
        public ReturnsType ReturnsType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 处理结果：PUPAWAY:退货入库;REDELIVERY:重新发货;RECLAIM-REDELIVERY:不要求归还并重新发货; REFUND:退款; COMPENSATION:不退货并赔偿。
        /// </summary>
        public HandlingType? HandlingType { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal ReturnsAmount { get; set; }

        /// <summary>
        /// 卖家承担运费
        /// </summary>
        public decimal? SellerPunishFee { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示处理状态 -1买家取消 0申请中 1等待买家退回 2买家已退，卖家等待收货 3卖家已收货，等待处理 4卖家已处理，等待买家确认 5流程结束。
        /// </summary>
        public RefundStatus Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandlingTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }


        /// <summary>
        /// 物流单号-商家再寄出
        /// </summary>
        public string ConsigneeExpressNo { get; set; }

        /// <summary>
        /// 物流公司-商家再寄出
        /// </summary>
        public string ConsigneeLogisticsName { get; set; }


        /// <summary>
        /// 物流单号-用户回寄
        /// </summary>
        public string ReturnExpressNo { get; set; }

        /// <summary>
        /// 物流公司-用户回寄
        /// </summary>
        public string ReturnLogisticsName { get; set; }


        /// <summary>
        /// 最新的处理流程记录编号
        /// </summary>
        public string NewStepLogId { get; set; }

    }
}
