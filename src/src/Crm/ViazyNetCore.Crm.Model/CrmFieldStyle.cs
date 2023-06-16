using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    public class CrmFieldStyle : EntityUpdate
    {
        public string Style { get; set; }
        public int Type { get; set; }
        public string FieldName { get; set; }
    }
}
