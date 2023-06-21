using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Consul.Internal.WatchProvider;

namespace ViazyNetCore.Consul
{
    public interface IClientWatchManager
    {
        Dictionary<string, HashSet<Watcher>> DataWatches { get; set; }
    }
}
