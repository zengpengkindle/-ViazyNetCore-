using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    /// <summary>
    /// 回款计划表
    /// </summary>
    public class CrmReceivablesPlan : EntityUpdate
    {
        /// <summary>
        /// 期数
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 回款ID
        /// </summary>
        public string ReceivablesId { get; set; }
        /// <summary>
        /// 1完成 0 未完成
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 计划回款金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 计划回款金额
        /// </summary>
        public DateTime ReturnDate { get; set; }
        /// <summary>
        /// 计划回款方式
        /// </summary>
        public string ReturnType { get; set; }
        /// <summary>
        /// 提前几天提醒
        /// </summary>
        public string Remind { get; set; }
        /// <summary>
        /// 提醒日期
        /// </summary>
        public DateTime RemindDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public long OwnerUserId { get; set; }
        /// <summary>
        /// 附件批次ID
        /// </summary>
        public string FileBatch { get; set; }
        /// <summary>
        /// 合同ID
        /// </summary>
        public long ContractId { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public long CustomerId { get; set; }
    }
}
