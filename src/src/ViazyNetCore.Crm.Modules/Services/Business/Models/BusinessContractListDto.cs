using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Models
{
    public class BusinessContractListDto
    {
        public long Id { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 合同名称
        /// </summary>
        public string ContractName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get;set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 0 未审核 1 审核通过 2 审核拒绝 3 审核中 4 已撤回 5草稿 6 作废
        /// </summary>
        public int CheckStatus { get; set; }
    }
}
