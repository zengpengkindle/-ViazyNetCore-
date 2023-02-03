using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Redis
{
    public interface IRedisSetCache
    {
        Task<bool> HashSetAsync(string redisKey, string key, string value);

        Task<bool> HashSetAsync<T>(string redisKey, string key, T value);

        Task<T?> HashGetAsync<T>(string redisKey, string key);
    }
}
