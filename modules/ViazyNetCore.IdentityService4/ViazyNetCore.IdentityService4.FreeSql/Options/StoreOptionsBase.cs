using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace ViazyNetCore.IdentityService4.FreeSql.Options
{
    public abstract class StoreOptionsBase
    {
        public string DefaultSchema { get; set; } = null;

        public string DbName { get; set; } = "master";

        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        public Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }
    }
}
