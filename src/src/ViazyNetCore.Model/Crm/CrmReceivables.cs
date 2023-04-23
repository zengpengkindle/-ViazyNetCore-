using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    /// <summary>
    /// 回款表
    /// </summary>
    public class CrmReceivables : EntityUpdate
    {
        /// <summary>
        /// 回款编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 回款计划I
        /// </summary>
        public string PlanId { get; set; }
        public long CustomerId { get; set; }
        public long ContractId { get; set; }
        /// <summary>
        /// 0 未审核 1 审核通过 2 审核拒绝 3 审核中 4 已撤回
        /// </summary>
        public int CheckStatus { get; set; }
        /// <summary>
        /// 审核记录ID
        /// </summary>
        public int ExamineRecordId { get; set; }
        public DateTime ReturnTime { get; set; }
        /// <summary>
        /// 回款方式
        /// </summary>
        public string ReturnType { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>
        public decimal Money { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 负责人ID
        /// </summary>
        public long OwnerUserId { get; set; }
        /// <summary>
        /// 批次备注
        /// </summary>
        public string  Remarks { get; set; }
        public string BatchId { get; set;}
    }
}
