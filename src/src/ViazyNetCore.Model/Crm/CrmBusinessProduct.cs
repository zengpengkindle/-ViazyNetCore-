using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    public class CrmBusinessProduct : EntityUpdate
    {
        public long BussinessId { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public int Num { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
        public string Unit { get; set; }
    }
}
