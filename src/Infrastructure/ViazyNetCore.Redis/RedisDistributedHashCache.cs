using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Redis
{
    public class RedisDistributedHashCache : RedisCache, IDistributedHashCache, IRedisCache
    {
        private readonly IRedisCache _redisCache;

        public RedisDistributedHashCache(IOptions<RedisCacheOptions> optionsAccessor, IRedisCache redisCache) : base(optionsAccessor)
        {
            this._redisCache = redisCache;
        }

        public Task Clear()
        {
            return this._redisCache.Clear();
        }

        public Task<bool> Exist(string key)
        {
            return this._redisCache.Exist(key);
        }

        public Task<TEntity?> Get<TEntity>(string key)
        {
            return this._redisCache.Get<TEntity>(key);
        }

        public Task<T?> HashGetAsync<T>(string redisKey, string key)
        {
            return this._redisCache.HashGetAsync<T>(redisKey, key);
        }

        public Task<bool> HashSetAsync(string redisKey, string key, string value)
        {
            return this._redisCache.HashSetAsync(redisKey, key, value);
        }

        public Task<bool> HashSetAsync<T>(string redisKey, string key, T value)
        {
            return this._redisCache.HashSetAsync<T>(redisKey, key, value);
        }

        public Task ListClearAsync(string redisKey)
        {
            return this._redisCache.ListClearAsync(redisKey);
        }

        public Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0)
        {
            return this._redisCache.ListDelRangeAsync(redisKey, redisValue, type);
        }

        public Task<T?> ListLeftPopAsync<T>(string redisKey) where T : class
        {
            return this._redisCache.ListLeftPopAsync<T>(redisKey);
        }

        public Task<string?> ListLeftPopAsync(string redisKey)
        {
            return this._redisCache.ListLeftPopAsync(redisKey);
        }

        public Task<long> ListLeftPushAsync(string redisKey, string redisValue)
        {
            return this._redisCache.ListLeftPushAsync(redisKey, redisValue);
        }

        public Task<long> ListLengthAsync(string redisKey)
        {
            return this._redisCache.ListLengthAsync(redisKey);
        }

        public Task<RedisValue[]> ListRangeAsync(string redisKey)
        {
            return this._redisCache.ListRangeAsync(redisKey);
        }

        public Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1)
        {
            return this._redisCache.ListRangeAsync(redisKey, db);
        }

        public Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop)
        {
            return this._redisCache.ListRangeAsync(redisKey, start, stop);
        }

        public Task<T?> ListRightPopAsync<T>(string redisKey) where T : class
        {
            return this._redisCache.ListRightPopAsync<T>(redisKey);
        }

        public Task<string?> ListRightPopAsync(string redisKey)
        {
            return this._redisCache.ListRightPopAsync(redisKey);
        }

        public Task<long> ListRightPushAsync(string redisKey, string redisValue)
        {
            return this._redisCache.ListRightPushAsync(redisKey, redisValue);
        }

        public Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue)
        {
            return this._redisCache.ListRightPushAsync(redisKey, redisValue);
        }

        public Task Set(string key, object value, TimeSpan cacheTime)
        {
            return this._redisCache.Set(key, value, cacheTime);
        }

        public Task SortedSetAddAsync(string redisKey, string redisValue, double score)
        {
            return this._redisCache.SortedSetAddAsync(redisKey, redisValue, score);
        }

        Task<string?> IRedisCache.Get(string key)
        {
            return this._redisCache.Get(key);
        }

        Task IRedisCache.Remove(string key)
        {
            return this._redisCache.Remove(key);
        }
    }
}
