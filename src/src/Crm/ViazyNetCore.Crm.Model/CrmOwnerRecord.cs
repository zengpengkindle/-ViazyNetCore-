using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    /// <summary>
    /// 负责人变更记录表
    /// </summary>
    public class CrmOwnerRecord : EntityAdd
    {
        /// <summary>
        /// 对象id
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 对象类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 上一负责人
        /// </summary>
        public long PreOwnerUserId { get; set; }
        /// <summary>
        /// 接手负责人
        /// </summary>
        public long PostOwnerUserId { get; set; }

    }
}
