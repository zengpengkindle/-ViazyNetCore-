using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Model.Payment
{
    public class BillPayments
    {

        /// <summary>
        /// 支付单号
        /// </summary>
        [Column(IsPrimary = true)]
        [StringLength(maximumLength: 20, ErrorMessage = "{0}不能超过{1}字")]
        public string PaymentId { get; set; }

        /// <summary>
        /// 资源编号
        /// </summary>
        [StringLength(maximumLength: 20, ErrorMessage = "{0}不能超过{1}字")]
        public string SourceId { get; set; }

        public string PayTitle { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 支付类型编码 关联payments.code
        /// </summary>
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public string PaymentCode { get; set; }

        /// <summary>
        /// 支付单生成IP
        /// </summary>
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public string Ip { get; set; }

        /// <summary>
        /// 支付的时候需要的参数，存的是json格式的一维数组
        /// </summary>
        [StringLength(maximumLength: 200, ErrorMessage = "{0}不能超过{1}字")]
        public string Parameters { get; set; }

        /// <summary>
        /// 支付回调后的状态描述
        /// </summary>
        [StringLength(maximumLength: 255, ErrorMessage = "{0}不能超过{1}字")]
        public string PayedMsg { get; set; }

        /// <summary>
        /// 第三方平台交易流水号
        /// </summary>
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public string TradeNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
