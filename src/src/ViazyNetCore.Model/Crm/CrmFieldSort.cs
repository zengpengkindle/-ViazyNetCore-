using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    public class CrmFieldSort : EntityAdd
    {
        public string Label { get; set; }
        public string FieldName { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Sort { get; set; }
        public int IsHide { get; set; }
        public long FieldId { get; set; }
    }
}
