using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model
{
    public class MemberThridAccount: EntityBase<long>
    {
        public string UnionId { get; set; }

        public long UserId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
