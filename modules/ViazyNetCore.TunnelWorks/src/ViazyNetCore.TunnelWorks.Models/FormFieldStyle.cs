using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Models
{
    public class FormFieldStyle : EntityUpdate
    {
        public string Style { get; set; }
        public int Type { get; set; }
        public string FieldName { get; set; }
    }
}
