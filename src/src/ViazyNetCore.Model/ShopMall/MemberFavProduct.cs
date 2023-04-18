using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.ShopMall
{
    public class MemberFavProduct : EntityBase
    {
        public string MemberId { get; set; }

        public string ProductId { get; set; }

        public string ProductOuterType { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
