using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Caching
{
    public class RuntimeMemoryCache : ICache
    {
        private readonly IMemoryCache _cache;

        public RuntimeMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Clear()
        {
            var l = GetCacheKeys();
            foreach (var s in l)
            {
                Remove(s);
            }
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public List<string> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            var keys = new List<string>();
            var cacheItems = entries as IDictionary;
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <returns></returns>
        public object Get(string cacheKey)
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));

            return _cache.Get(cacheKey);
        }

        public T Get<T>(string cacheKey)
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));

            return _cache.Get<T>(cacheKey);
        }

        public void MarkDeletion(string key, object value, TimeSpan timeSpan)
        {
            Remove(key);
        }

        public void Remove(string cacheKey)
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));

            _cache.Remove(cacheKey);
        }

        public void Set(string cacheKey, object value, TimeSpan expiresIn)
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));
            if (value == null)
                return;//throw new ArgumentNullException(nameof(value));

            _cache.Set(cacheKey, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiresIn));
        }
    }


}
