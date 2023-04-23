using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    /// <summary>
    /// 合同
    /// </summary>
    public class CrmContract : EntityUpdate
    {
        /// <summary>
        /// 合同名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 商机ID
        /// </summary>
        public long BusinessId { get; set; }
        /// <summary>
        /// 0 未审核 1 审核通过 2 审核拒绝 3 审核中 4 已撤回 5草稿 6 作废
        /// </summary>
        public int CheckStatus { get; set; }
        /// <summary>
        /// 审核记录ID
        /// </summary>
        public long ExamineRecordId { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime OrderDate { get; set; }
        public long OwnerUserId { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 整单折扣
        /// </summary>
        public decimal DiscountRate { get; set; }
        /// <summary>
        /// 产品总金额
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public string Types { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// 批次 比如附件批次
        /// </summary>
        public long BatchId { get; set; }
        /// <summary>
        /// 只读权限
        /// </summary>
        public long RoUserId { get; set; }
        /// <summary>
        /// 读写权限
        /// </summary>
        public long RwUserId { get; set; }
        /// <summary>
        /// 客户签约人（联系人id）
        /// </summary>
        public long ContactsId { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 公司签约人
        /// </summary>
        public long CompanyUserId { get; set; }
    }
}
