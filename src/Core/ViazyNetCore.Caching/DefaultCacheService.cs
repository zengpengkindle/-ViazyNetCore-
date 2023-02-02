using Nito.AsyncEx;

namespace ViazyNetCore.Caching
{
    [Serializable]
    public class DefaultCacheService : ICacheService
    {
        private readonly ICache _cache;
        private readonly Dictionary<CachingExpirationType, TimeSpan> _cachingExpirationDictionary;
        private readonly ICache _localCache;
        private readonly AsyncLock _mutex = new();

        public bool EnableDistributedCache { get; }


        public DefaultCacheService(ICache cache, ICache localCache, float cacheExpirationFactor, bool enableDistributedCache)
        {
            _cache = cache;
            _localCache = localCache;
            EnableDistributedCache = enableDistributedCache;
            _cachingExpirationDictionary = new Dictionary<CachingExpirationType, TimeSpan>
            {
                { CachingExpirationType.Invariable, new TimeSpan(0, 0, (int)(86400f * cacheExpirationFactor)) },
                { CachingExpirationType.Stable, new TimeSpan(0, 0, (int)(28800f * cacheExpirationFactor)) },
                { CachingExpirationType.RelativelyStable, new TimeSpan(0, 0, (int)(7200f * cacheExpirationFactor)) },
                { CachingExpirationType.UsualSingleObject, new TimeSpan(0, 0, (int)(600f * cacheExpirationFactor)) },
                { CachingExpirationType.UsualObjectCollection, new TimeSpan(0, 0, (int)(300f * cacheExpirationFactor)) },
                { CachingExpirationType.SingleObject, new TimeSpan(0, 0, (int)(180f * cacheExpirationFactor)) },
                { CachingExpirationType.ObjectCollection, new TimeSpan(0, 0, (int)(180f * cacheExpirationFactor)) }
            };
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public object Get(string cacheKey)
        {
            object? obj2 = null;
            if (EnableDistributedCache)
            {
                obj2 = _localCache.Get(cacheKey);
            }
            if (obj2 == null)
            {
                obj2 = _cache.Get(cacheKey);
                if (EnableDistributedCache)
                {
                    _localCache.Set(cacheKey, obj2, _cachingExpirationDictionary[CachingExpirationType.SingleObject]);
                }
            }
            return obj2;
        }

        public T Get<T>(string cacheKey)
        {
            T obj2 = default;
            if (EnableDistributedCache)
            {
                obj2 = _localCache.Get<T>(cacheKey);
            }
            if (obj2 == null)
            {
                obj2 = _cache.Get<T>(cacheKey);

                if (EnableDistributedCache)
                {
                    _localCache.Set(cacheKey, obj2, _cachingExpirationDictionary[CachingExpirationType.SingleObject]);
                }
            }
            return obj2;
        }

        public object GetFromFirstLevel(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public T GetFromFirstLevel<T>(string cacheKey)
        {
            return _cache.Get<T>(cacheKey);
        }

        public void MarkDeletion(string cacheKey, object entity, CachingExpirationType cachingExpirationType)
        {
            _cache.MarkDeletion(cacheKey, entity, _cachingExpirationDictionary[cachingExpirationType]);
        }

        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);
            if (EnableDistributedCache)
            {
                _localCache.Remove(cacheKey);
            }
        }

        public void Set(string cacheKey, object value, TimeSpan timeSpan)
        {
            _cache.Set(cacheKey, value, timeSpan);
            if (EnableDistributedCache)
            {
                _localCache.Set(cacheKey, value, _cachingExpirationDictionary[CachingExpirationType.SingleObject]);
            }
        }

        public void Set(string cacheKey, object value, CachingExpirationType cachingExpirationType)
        {
            Set(cacheKey, value, _cachingExpirationDictionary[cachingExpirationType]);
        }

        public T LockGet<T>(string cacheKey, Func<T> setFunc, CachingExpirationType cachingExpirationType)
        {
            var result = Get<T>(cacheKey);
            if (result == null)
            {
                using (_mutex.Lock())
                {
                    result = Get<T>(cacheKey);
                    if (result == null)
                    {
                        result = setFunc();
                        Set(cacheKey, result, cachingExpirationType);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 直接缓存redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="setFunc"></param>
        /// <param name="cachingExpirationType"></param>
        /// <returns></returns>
        public async Task<T> LockGetFirstLevelAsync<T>(string cacheKey, Func<Task<T>> setFunc, CachingExpirationType cachingExpirationType)
        {
            var result = GetFromFirstLevel<T>(cacheKey);
            if (result == null)
            {
                using (await _mutex.LockAsync())
                {
                    result = GetFromFirstLevel<T>(cacheKey);
                    if (result == null)
                    {
                        result = await setFunc();
                        Set(cacheKey, result, cachingExpirationType);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 先取本地缓存后redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="setFunc"></param>
        /// <param name="cachingExpirationType"></param>
        /// <returns></returns>
        public async Task<T> LockGetAsync<T>(string cacheKey, Func<Task<T>> setFunc, CachingExpirationType cachingExpirationType)
        {
            var result = Get<T>(cacheKey);
            if (result == null)
            {
                using (await _mutex.LockAsync())
                {
                    result = Get<T>(cacheKey);
                    if (result == null)
                    {
                        result = await setFunc();
                        Set(cacheKey, result, cachingExpirationType);
                    }
                }
            }
            return result;
        }
    }

}
