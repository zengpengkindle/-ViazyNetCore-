namespace ViazyNetCore.Caching
{
    public interface ICacheService
    {
        void Clear();
        object Get(string cacheKey);
        T Get<T>(string cacheKey);
        T LockGet<T>(string cacheKey, Func<T> setFunc, CachingExpirationType cachingExpirationType);
        Task<T> LockGetFirstLevelAsync<T>(string cacheKey, Func<Task<T>> setFunc, CachingExpirationType cachingExpirationType);
        Task<T> LockGetAsync<T>(string cacheKey, Func<Task<T>> setFunc, CachingExpirationType cachingExpirationType);
        object GetFromFirstLevel(string cacheKey);
        T GetFromFirstLevel<T>(string cacheKey);
        void Remove(string cacheKey);
        void Set(string cacheKey, object value, TimeSpan timeSpan);
        void Set(string cacheKey, object value, CachingExpirationType cachingExpirationType);

        bool EnableDistributedCache { get; }
    }
}