using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    /// <summary>
    /// 联系人表
    /// </summary>
    public class CrmContacts : EntityUpdate
    {
        /// <summary>
        /// 联系人名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime NextTime { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Post { get; set; }
        public long CustomerId { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 负责人ID
        /// </summary>
        public long OwnerUserId { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string BatchId { get; set; }
    }
}
