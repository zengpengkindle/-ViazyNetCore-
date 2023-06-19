using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    public class CrmCustomerSettingUser : EntityUpdate
    {
        /// <summary>
        /// 客户规则限制ID
        /// </summary>
        public long SettingId { get; set; }
        public long UserId { get; set; }
        public long DeptId { get; set; }
        /// <summary>
        /// 1 员工 2 部门
        /// </summary>
        public int Type { get; set; }
    }
}
