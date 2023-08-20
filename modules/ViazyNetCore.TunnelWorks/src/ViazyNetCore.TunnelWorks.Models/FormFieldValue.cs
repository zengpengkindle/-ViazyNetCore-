using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Models
{
    public class FormFieldValue : EntityAdd
    {
        public string FieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public long BatchId { get; set; }
    }
}
