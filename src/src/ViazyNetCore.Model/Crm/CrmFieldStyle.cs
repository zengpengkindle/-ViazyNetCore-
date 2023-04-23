using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    public class CrmFieldStyle : EntityUpdate
    {
        public string Style { get; set; }
        public int Type { get; set; }
        public string FieldName { get; set; }
    }
}
