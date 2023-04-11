using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViazyNetCore.Model.Payment
{
    public class BillRefund
    {
        /// <summary>
        /// 退款单ID
        /// </summary>
        [Display(Name = "退款单ID")]
        [Column(IsPrimary = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string RefundId { get; set; }
        /// <summary>
        /// 售后单id
        /// </summary>
        [Display(Name = "售后单id")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string AftersalesId { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        [Display(Name = "退款金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal Money { get; set; }
        /// <summary>
        /// 用户ID 关联user.id
        /// </summary>
        [Display(Name = "用户ID 关联user.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int UserId { get; set; }
        /// <summary>
        /// 资源id，根据type不同而关联不同的表
        /// </summary>
        [Display(Name = "资源id，根据type不同而关联不同的表")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string SourceId { get; set; }
        /// <summary>
        /// 资源类型1=订单,2充值单
        /// </summary>
        [Display(Name = "资源类型1=订单,2充值单")]
        [Required(ErrorMessage = "请输入{0}")]
        public int Type { get; set; }
        /// <summary>
        /// 退款支付类型编码 默认原路返回 关联支付单表支付编码
        /// </summary>
        [Display(Name = "退款支付类型编码 默认原路返回 关联支付单表支付编码")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string PaymentCode { get; set; }
        /// <summary>
        /// 第三方平台交易流水号
        /// </summary>
        [Display(Name = "第三方平台交易流水号")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string TradeNo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int Status { get; set; }
        /// <summary>
        /// 退款失败原因
        /// </summary>
        [Display(Name = "退款失败原因")]
        public string Memo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? UpdateTime { get; set; }
    }
}
