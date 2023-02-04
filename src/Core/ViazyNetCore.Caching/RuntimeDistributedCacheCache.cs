using Microsoft.Extensions.Caching.Distributed;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Caching
{
    public class RuntimeDistributedCacheCache : ICache
    {
        private readonly IDistributedCache _distributedCache;

        public RuntimeDistributedCacheCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public object? Get(string cacheKey)
        {
            return this._distributedCache.Get(cacheKey);
        }

        public T? Get<T>(string cacheKey)
        {
            return this._distributedCache.Get<T>(cacheKey);
        }

        public void MarkDeletion(string cacheKey, object value, TimeSpan expiresIn)
        {
            this.Set(cacheKey, value, expiresIn);
            this._distributedCache.Refresh(cacheKey);
            //this.Remove(cacheKey);
        }

        public void Remove(string cacheKey)
        {
            this._distributedCache.Remove(cacheKey);
        }

        public void Set(string cacheKey, object value, TimeSpan expiresIn)
        {
            this._distributedCache.Set(cacheKey, value.Object2Bytes(), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expiresIn });
        }
    }
}
