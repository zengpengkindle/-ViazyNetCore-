using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using ViazyNetCore.Redis;

namespace ViazyNetCore.Caching
{
    public class RedisDistributedHashCache : RedisCache, IDistributedHashCache
    {
        public RedisDistributedHashCache(IOptions<RedisCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }
    }
}
