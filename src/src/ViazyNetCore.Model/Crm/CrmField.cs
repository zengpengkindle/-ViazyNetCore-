using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    public class CrmField : EntityUpdate
    {
        public string FieldName { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Lable { get; set; }
        public string Remark { get; set; }
        public string InputTips { get; set; }
        public string MaxLength { get; set; }
        public string DefaultValue { get; set; }
        public int IsUnique { get; set; }
        public string IsNull { get; set; }
        public int Sorting { get; set; }
        public string Options { get; set; }
        public int Operating { get; set; }
        public int ExamineCategoryId { get; set; }
        public int FieldType { get; set; }
        public int Relevant { get; set; }
    }
}
