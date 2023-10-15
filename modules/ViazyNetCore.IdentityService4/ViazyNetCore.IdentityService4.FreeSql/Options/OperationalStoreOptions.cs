using System;
using FreeSql;

namespace ViazyNetCore.IdentityService4.FreeSql.Options
{
    public class OperationalStoreOptions : StoreOptionsBase
    {
        public TableConfiguration PersistedGrants { get; set; } = new TableConfiguration("PersistedGrants");

        public TableConfiguration DeviceFlowCodes { get; set; } = new TableConfiguration("DeviceCodes");

        public bool EnableTokenCleanup { get; set; } = false;

        public int TokenCleanupInterval { get; set; } = 3600;

        public int TokenCleanupBatchSize { get; set; } = 100;
    }
}