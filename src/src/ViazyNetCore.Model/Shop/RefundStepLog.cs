
namespace ViazyNetCore.Model
{
    /// <summary>
    /// 退换货流程记录
    /// </summary>
    [Table(Name = "ShopMall.RefundStepLog")]
    public partial class RefundStepLog : EntityBase<string>
    {
        /// <summary>
        /// 对应退换单-子订单编号
        /// </summary>
        public string RefundTradeId { get; set; }

        /// <summary>
        /// 流程计数
        /// </summary>
        public int StepIndex { get; set; }

        /// <summary>
        /// 所属流程编号
        /// </summary>
        public string StepId { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 流程提示
        /// </summary>
        public string Remind { get; set; }

        /// <summary>
        /// 附加说明
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 处理人-（用户时为memberId 商家时为shopId）
        /// </summary>
        public string HandleUserId { get; set; }

        /// <summary>
        /// 处理人类型-0商家1用户
        /// </summary>
        public RefundTradeLogType HandleUserType { get; set; }

        /// <summary>
        /// 上一个流程编号
        /// </summary>
        public string PreStepLogId { get; set; }

        /// <summary>
        /// 下一个流程编号
        /// </summary>
        public string NextStepLogId { get; set; }

        /// <summary>
        /// 最新的处理流程记录编号
        /// </summary>
        public string NewStepLogId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandleTime { get; set; }

    }
}
