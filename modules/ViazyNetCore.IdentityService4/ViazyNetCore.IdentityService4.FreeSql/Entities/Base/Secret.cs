using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.IdentityService4.FreeSql.Entities
{
    public abstract class Secret : Entity<long>
    {
        [Column(StringLength = 1000, IsNullable = true)]
        public string Description { get; set; }

        [Column(StringLength = 4000, IsNullable = false)]
        public string Value { get; set; }

        [Column(IsNullable = true)]
        public DateTime? Expiration { get; set; }

        [Column(StringLength = 255, IsNullable = false)]
        public string Type { get; set; } = "SharedSecret";
    }
}
