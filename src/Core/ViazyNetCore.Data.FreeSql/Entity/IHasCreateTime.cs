using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Data.FreeSql
{
    public interface IHasCreateTime
    {
        public DateTime? CreateTime { get; set; }
    }
}
