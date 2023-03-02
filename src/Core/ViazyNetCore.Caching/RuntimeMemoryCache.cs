using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Caching
{
    public class RuntimeMemoryCache : ICache
    {
        private readonly IMemoryCache _cache;

        public RuntimeMemoryCache(IMemoryCache cache)
        {
            this._cache = cache;
        }

        public void Clear()
        {
            var l = this.GetCacheKeys();
            foreach (var s in l)
            {
                this.Remove(s);
            }
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public List<string?> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = this._cache.GetType().GetField("_entries", flags)?.GetValue(this._cache);
            var keys = new List<string?>();
            if (entries is not IDictionary cacheItems) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key?.ToString());
            }
            return keys;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <returns></returns>
        public object? Get(string cacheKey)
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));

            return this._cache.Get(cacheKey);
        }

        public T? Get<T>(string cacheKey)
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));

            return this._cache.Get<T>(cacheKey);
        }

        public void MarkDeletion(string key, object value, TimeSpan timeSpan)
        {
            this.Remove(key);
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

        public Task RemoveByPatternAsync(string pattern)
        {
            if (pattern.IsNull())
                return Task.CompletedTask;
            pattern = Regex.Replace(pattern, @"\{.*\}", "*");
            var cacheKeys = GetCacheKeys();
            var keys = cacheKeys.Where(k => Regex.IsMatch(k, pattern)).ToList();
            foreach (var key in keys)
            {
                if (key != null)
                    _cache.Remove(key);
            }
            return Task.CompletedTask;
            //if (pattern.IsNull())
            //    return default;

            //pattern = Regex.Replace(pattern, @"\{.*\}", "*");

            //_cache.TryGetValue(pattern, out var result);
            //throw new NotImplementedException();
        }
    }


}
