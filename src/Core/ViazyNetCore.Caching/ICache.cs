namespace ViazyNetCore.Caching
{
    public interface ICache
    {
        void Clear();
        object? Get(string cacheKey);
        T? Get<T>(string cacheKey);
        void MarkDeletion(string cacheKey, object value, TimeSpan expiresIn);
        void Remove(string cacheKey);
        Task RemoveByPatternAsync(string cacheKeyPattern);
        void Set(string cacheKey, object value, TimeSpan expiresIn);
    }

}
