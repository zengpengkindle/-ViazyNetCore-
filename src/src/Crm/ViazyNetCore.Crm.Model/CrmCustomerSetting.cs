using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    /// <summary>
    /// 员工拥有以及锁定客户数限制
    /// </summary>
    public class CrmCustomerSetting : EntityUpdate
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 可拥有客户数量
        /// </summary>
        public int CustomerNum { get; set; }
        /// <summary>
        /// 成交客户是否占用数量 0 不占用 1 占用
        /// </summary>
        public int CustomerDeal { get; set; }
        /// <summary>
        /// 类型 1 拥有客户数限制 2 锁定客户数限制
        /// </summary>
        public int Type { get; set; }
    }
}
