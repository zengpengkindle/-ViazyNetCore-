using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    /// <summary>
    /// 活动记录
    /// </summary>
    public class CrmActionRecord : EntityUpdate
    {
        /// <summary>
        /// 记录类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public long ActionId { get; set; }

        /// <summary>
        /// 活动内容
        /// </summary>
        public string Content { get; set; }
    }
}
