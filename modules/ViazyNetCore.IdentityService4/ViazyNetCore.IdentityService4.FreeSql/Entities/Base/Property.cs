using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.IdentityService4.FreeSql.Entities
{
    public abstract class Property : Entity<long>
    {
        [Column(StringLength = 255, IsNullable = false)]
        public string Key { get; set; }

        [Column(StringLength = 2000, IsNullable = false)]
        public string Value { get; set; }
    }
}
