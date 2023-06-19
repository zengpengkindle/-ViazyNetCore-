using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Consul
{
    public interface IConsulClientProvider
    {
        ValueTask<ConsulClient> GetClient();

        ValueTask<IEnumerable<ConsulClient>> GetClients();

        ValueTask Check();
    }
}
