using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Consul.Internal.WatchProvider
{
    public abstract class WatcherBase : Watcher
    {
        protected WatcherBase()
        {

        }

        public override async Task Process()
        {
            await ProcessImpl();
        }

        protected abstract Task ProcessImpl();
    }
}
