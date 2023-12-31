﻿namespace ViazyNetCore.Caching
{
    public static class CacheExtension
    {
        public static T Get<T>(this ICache cache, string cacheKey, Func<T> action, TimeSpan expiresIn)
        {
            var result = cache.Get<T>(cacheKey);
            if (result == null)
            {
                result = action();
                if (result != null)
                    cache.Set(cacheKey, result, expiresIn);
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
        public static T Get<T>(this ICache cache, string cacheKey, Func<T> action)
        {
            var result = cache.Get<T>(cacheKey);
            if (result == null)
            {
                result = action();
                if (result != null)
                    cache.Set(cacheKey, result, new TimeSpan(0, 0, 86400));
            }
            return result;
        }

        public static async Task<T> GetAsync<T>(this ICache cache, string cacheKey, Func<Task<T>> action, TimeSpan expiresIn)
        {
            var result = cache.Get<T>(cacheKey);
            if (result == null)
            {
                Task<T> tresult = action();
                if (tresult != null)
                    result = await tresult;
                if (result != null)
                    cache.Set(cacheKey, result, expiresIn);
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
        public static async Task<T> GetAsync<T>(this ICache cache, string cacheKey, Func<Task<T>> action)
        {
            var result = cache.Get<T>(cacheKey);
            if (result == null)
            {
                using (GA.Lock(cacheKey, TimeSpan.FromSeconds(30)))
                {
                    Task<T> tresult = action();
                    if (tresult != null)
                        result = await tresult;
                    if (result != null)
                        cache.Set(cacheKey, result, new TimeSpan(0, 0, 86400));
                }
            }
            return result;
        }
    }
}