using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Models
{
    /// <summary>
    /// 产品分类表
    /// </summary>
    public class FormTemplateCategory : EntityUpdate
    {
        public string Name { get; set; }
        public string TemplateId { get; set; }
        public int Sort { get; set; }
    }
}
