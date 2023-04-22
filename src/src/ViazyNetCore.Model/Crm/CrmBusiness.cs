using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    /// <summary>
    /// 表示一个商机
    /// </summary>
    public class CrmBusiness : EntityUpdate
    {
        public int TypeId { get; set; }

        public int StatusId { get; set; }

        public DateTime? NextTime { get; set; }

        public long CustomerId { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime DealTime { get; set; }

        /// <summary>
        /// 公司企业名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        public decimal Money { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Remark { get; set; }

        public long OwnerUserId { get; set; }

        public long BatchId { get; set; }

        public bool IsEnd { get; set; }

        public long RoUserId { get; set; }

        public long RwUserId { get; set; }

        public string StatusRemark { get; set; }
    }
}
