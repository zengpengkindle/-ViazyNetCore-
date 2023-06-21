using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Consul.Internal.WatchProvider
{
    public abstract class Watcher
    {
        protected Watcher()
        {
        }

        public abstract Task Process();

        public static class Event
        {
            public enum KeeperState
            {
                Disconnected = 0,
                SyncConnected = 3,
            }
        }
    }
}
