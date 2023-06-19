using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    /// <summary>
    /// 线索表
    /// </summary>
    public class CrmLeads : EntityUpdate
    {
        /// <summary>
        /// 1已转化 0 未转化
        /// </summary>
        public int IsTransform { get; set; }
        /// <summary>
        /// 跟进状态 0未跟进1已跟进
        /// </summary>
        public int Followup { get; set; }
        /// <summary>
        /// 线索名称
        /// </summary>
        public string LeadsName { get; set; }
        public int CustomerId { get; set; }
        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime NextTime { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public long OwnerUserId { get; set; }
        /// <summary>
        /// 批次 比如附件批次
        /// </summary>
        public string BatchId { get; set; }
        public string LastContent { get; set; }
    }
}
