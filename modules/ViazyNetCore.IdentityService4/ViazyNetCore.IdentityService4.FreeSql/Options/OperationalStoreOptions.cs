using System;
using FreeSql;

namespace ViazyNetCore.IdentityService4.FreeSql.Options
{
    public class OperationalStoreOptions
    {
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        public Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }

        public string DefaultSchema { get; set; } = null;

        public TableConfiguration PersistedGrants { get; set; } = new TableConfiguration("PersistedGrants");

        public TableConfiguration DeviceFlowCodes { get; set; } = new TableConfiguration("DeviceCodes");

        public bool EnableTokenCleanup { get; set; } = false;

        public int TokenCleanupInterval { get; set; } = 3600;

        public int TokenCleanupBatchSize { get; set; } = 100;
    }
}