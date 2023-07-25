using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Models
{
    public class FormFieldValue : EntityAdd
    {
        public long FieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string BatchId { get; set; }
    }
}
