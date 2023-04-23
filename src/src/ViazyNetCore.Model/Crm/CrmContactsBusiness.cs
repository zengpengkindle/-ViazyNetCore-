using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    /// <summary>
    /// 商机联系人关联表
    /// </summary>
    public class CrmContactsBusiness : EntityAdd
    {
        public long BusinessId { get; set; }
        public long ContactsId { get; set; }
    }
}
