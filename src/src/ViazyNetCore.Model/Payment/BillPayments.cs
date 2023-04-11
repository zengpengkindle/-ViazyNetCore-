using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Model.Payment
{
    public class BillPayments
    {

        /// <summary>
        /// 支付单号
        /// </summary>
        [Display(Name = "支付单号")]
        [Column(IsPrimary = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 20, ErrorMessage = "{0}不能超过{1}字")]
        public string PaymentId { get; set; }

        /// <summary>
        /// 资源编号
        /// </summary>
        [Display(Name = "资源编号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 20, ErrorMessage = "{0}不能超过{1}字")]
        public string SourceId { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Display(Name = "支付金额")]
        [Required(ErrorMessage = "请输入{0}")]

        public decimal Money { get; set; }

        /// <summary>
        /// 用户ID 关联user.id
        /// </summary>
        [Display(Name = "用户ID 关联user.id")]
        [Required(ErrorMessage = "请输入{0}")]

        public int UserId { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        [Display(Name = "单据类型")]
        [Required(ErrorMessage = "请输入{0}")]

        public int Type { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        [Display(Name = "支付状态")]
        [Required(ErrorMessage = "请输入{0}")]

        public int Status { get; set; }

        /// <summary>
        /// 支付类型编码 关联payments.code
        /// </summary>
        [Display(Name = "支付类型编码 关联payments.code")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public string PaymentCode { get; set; }

        /// <summary>
        /// 支付单生成IP
        /// </summary>
        [Display(Name = "支付单生成IP")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public string Ip { get; set; }

        /// <summary>
        /// 支付的时候需要的参数，存的是json格式的一维数组
        /// </summary>
        [Display(Name = "支付的时候需要的参数，存的是json格式的一维数组")]
        [StringLength(maximumLength: 200, ErrorMessage = "{0}不能超过{1}字")]
        public string Parameters { get; set; }

        /// <summary>
        /// 支付回调后的状态描述
        /// </summary>
        [Display(Name = "支付回调后的状态描述")]
        [StringLength(maximumLength: 255, ErrorMessage = "{0}不能超过{1}字")]
        public string PayedMsg { get; set; }

        /// <summary>
        /// 第三方平台交易流水号
        /// </summary>
        [Display(Name = "第三方平台交易流水号")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public string TradeNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]

        public DateTime? UpdateTime { get; set; }
        public string PayTitle { get; set; }
    }
}
