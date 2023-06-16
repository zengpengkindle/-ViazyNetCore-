using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    /// <summary>
    /// 商机阶段变化表
    /// </summary>
    public class CrmBusinessChange : EntityUpdate
    {
        public long BusinessId { get; set; }

        /// <summary>
        /// 阶段ID
        /// </summary>
        public int StatusId { get; set; }
    }
}
