namespace ViazyNetCore.Model
{
    /// <summary>
    /// 退换货业务流程配置
    /// </summary>
    [Table(Name = "ShopMall.RefundStepConfig")]
    public partial class RefundStepConfig : EntityBase<string>
    {
        /// <summary>
        /// 设置或获取一个值，表示所属业务流程组。
        /// </summary>
        public string StepSet { get; set; }

        /// <summary>
        /// 业务组名
        /// </summary>
        public string SetName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示步骤名。
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 是否流程组第一步
        /// </summary>
        public bool StepTop { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示可转入步骤。
        /// </summary>
        public string NextStepIds { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示处理提示。
        /// </summary>
        public string Remind { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示进入该步骤后对应退换单应变更的状态。
        /// </summary>
        public RefundStatus SetRefundTradeStatus { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示步骤处理人类型 0商家1用户。
        /// </summary>
        public RefundTradeLogType HandleUserType { get; set; }

        /// <summary>
        /// 是否需要填写参数-用户回寄地址信息
        /// </summary>
        public bool ShowRefund { get; set; }

        /// <summary>
        /// 是否需要填写参数-快递信息
        /// </summary>
        public bool ShowLogistic { get; set; }

        /// <summary>
        /// 是否需要填写参数-退货产生的财务信息
        /// </summary>
        public bool ShowFinance { get; set; }


    }
}
