namespace ViazyNetCore.Caching
{
    public static class CacheExtension
    {
        public static T Get<T>(this ICacheService cache, string cacheKey, Func<T> action, CachingExpirationType cachingExpirationType)
        {
            T result = default;
            result = cache.Get<T>(cacheKey);
            if (result == null)
            {
                result = action();
                if (result != null)
                    cache.Set(cacheKey, result, cachingExpirationType);
            }
            return result;
        }
        /// <summary>
        /// 永久储存的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="cacheKey"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T Get<T>(this ICacheService cache, string cacheKey, Func<T> action)
        {
            T result = default;
            result = cache.Get<T>(cacheKey);
            if (result == null)
            {
                result = action();
                if (result != null)
                    cache.Set(cacheKey, result, CachingExpirationType.Invariable);
            }
            return result;
        }

        public static async Task<T> GetAsync<T>(this ICacheService cache, string cacheKey, Func<Task<T>> action, CachingExpirationType cachingExpirationType)
        {
            T result = default;
            result = cache.Get<T>(cacheKey);
            if (result == null)
            {
                Task<T> tresult = action();
                if (tresult != null)
                    result = await tresult;
                if (result != null)
                    cache.Set(cacheKey, result, cachingExpirationType);
            }
            return result;
        }
        /// <summary>
        /// 永久储存的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="cacheKey"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this ICacheService cache, string cacheKey, Func<Task<T>> action)
        {
            T result = default;
            result = cache.Get<T>(cacheKey);
            if (result == null)
            {
                Task<T> tresult = action();
                if (tresult != null)
                    result = await tresult;
                if (result != null)
                    cache.Set(cacheKey, result, CachingExpirationType.Invariable);
            }
            return result;
        }
    }
}