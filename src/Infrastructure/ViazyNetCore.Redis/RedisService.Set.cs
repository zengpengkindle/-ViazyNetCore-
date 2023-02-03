using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace ViazyNetCore.Redis
{
    public partial class RedisService : IRedisSetCache
    {
        public Task<bool> HashSetAsync(string redisKey, string key, string value)
        {
            return this._database.HashSetAsync(redisKey, key, value);
        }

        public Task<bool> HashSetAsync<T>(string redisKey, string key, T value)
        {
            var redisValue = JsonConvert.SerializeObject(value);
            return this._database.HashSetAsync(redisKey, key, redisValue);
        }

        public async Task<T?> HashGetAsync<T>(string redisKey, string key)
        {
            var value = await this._database.HashGetAsync(redisKey, key);
            if (value == RedisValue.Null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value!);
        }
    }
}
