using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using ViazyNetCore.Redis;

namespace ViazyNetCore.Caching
{
    public interface IDistributedHashCache : IDistributedCache, IRedisCache
    {

    }
}
