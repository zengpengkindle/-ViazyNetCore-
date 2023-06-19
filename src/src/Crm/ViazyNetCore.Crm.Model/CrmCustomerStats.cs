using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    /// <summary>
    /// 每日客户统计
    /// </summary>
    public class CrmCustomerStats: EntityAdd
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 客户总数
        /// </summary>
        public int CustomerNum { get; set;}
    }
}
