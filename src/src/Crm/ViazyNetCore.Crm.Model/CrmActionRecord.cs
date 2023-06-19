using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.Aop;

namespace ViazyNetCore.Crm.Model
{
    /// <summary>
    /// oa操作记录表
    /// </summary>
    public class CrmActionRecord : EntityAdd
    {
        /// <summary>
        /// 记录类型
        /// </summary>
        public CrmType Type { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public long ActionId { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 抄送人IDs
        /// </summary>
        public string JoinUserIds { get; set; }
        /// <summary>
        /// 抄送部门IDs
        /// </summary>
        public string DeptIds { get; set; }
    }
}
