using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.MultiTenancy
{
    public class TenantConfiguration
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string TableRule { get; set; }

        public TenantConfiguration()
        {
            IsActive = true;
        }

        public TenantConfiguration(int id, [NotNull] string name) : this()
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
        }
    }
}
