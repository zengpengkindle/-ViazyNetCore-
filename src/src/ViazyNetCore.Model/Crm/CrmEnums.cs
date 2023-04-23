using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    public enum CrmType
    {
        /// <summary>
        /// 线索
        /// </summary>
        [Description("线索")]
        Leads = 1,
        /// <summary>
        /// 客户
        /// </summary>
        [Description("客户")]
        Customer = 2,
        /// <summary>
        /// 联系人
        /// </summary>
        [Description("联系人")]
        Contacts =3,
        /// <summary>
        /// 产品
        /// </summary>
        [Description("产品")]
        Product =4,
        /// <summary>
        /// 商机
        /// </summary>
        [Description("商机")]
        Business =5,
        /// <summary>
        /// 合同
        /// </summary>
        [Description("合同")]
        Contract =6,
        /// <summary>
        /// 回款
        /// </summary>
        [Description("回款")]
        Receivables =7,
        /// <summary>
        /// 回款计划
        /// </summary>
        [Description("回款计划")]
        ReceivablesPlan =8,
        /// <summary>
        /// 客户公海
        /// </summary>
        [Description("客户公海")]
        CustomerPool =9
    }
}
