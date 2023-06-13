using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.apache.zookeeper;

namespace ViazyNetCore.Zookeeper
{
    public interface IZookeeperClientProvider
    {
        ValueTask<(ManualResetEvent, ZooKeeper)> GetZooKeeper();

        ValueTask<IEnumerable<(ManualResetEvent, ZooKeeper)>> GetZooKeepers();

        ValueTask Check();
    }
}
