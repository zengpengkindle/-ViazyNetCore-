using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model
{
    [Table(Name = "ShopMall.Credits")]
    public class Credits : EntityBase<string>
    {
        public CreditType CreditType { get; set; }

        public ComStatus Status { get; set; }

        public string Name { get; set; }

        public string CreditKey { get; set; }

        public string Exdata { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
