using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    /// <summary>
    /// 产品分类表
    /// </summary>
    public class CrmProductCategory : EntityUpdate
    {
        public string Name { get; set; }
        public string ProductId { get; set; }
    }
}
