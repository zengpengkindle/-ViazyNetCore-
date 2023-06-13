using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.apache.zookeeper;

namespace ViazyNetCore.Zookeeper
{
    public class DefaultZookeeperClientProvider : IZookeeperClientProvider
    {
        public ValueTask Check()
        {
            throw new NotImplementedException();
        }

        public ValueTask<(ManualResetEvent, ZooKeeper)> GetZooKeeper()
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<(ManualResetEvent, ZooKeeper)>> GetZooKeepers()
        {
            throw new NotImplementedException();
        }
    }
}
