using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.Caching
{
    public class MemoryDistributedHashCache : IDistributedHashCache
    {
        private static readonly Task CompletedTask = Task.CompletedTask;

        private readonly IMemoryCache _memCache;

        class CacheItem
        {
            private readonly Lazy<ConcurrentDictionary<string, byte[]>> _lazyFields;
            public ConcurrentDictionary<string, byte[]> Fields => this._lazyFields.Value;

            public byte[] Data { get; }

            public CacheItem(byte[] data)
            {
                this._lazyFields = new Lazy<ConcurrentDictionary<string, byte[]>>(() => new ConcurrentDictionary<string, byte[]>());
                this.Data = data;
            }
        }

        public MemoryDistributedHashCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor)
        {
            if (optionsAccessor is null)
                throw new ArgumentNullException(nameof(optionsAccessor));

            this._memCache = new MemoryCache(optionsAccessor.Value);
        }

        public byte[]? Get(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            var item = this._memCache.Get(key) as CacheItem;
            return item?.Data;
        }

        public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            return Task.FromResult(this.Get(key));
        }


        private static MemoryCacheEntryOptions ParseOptions(DistributedCacheEntryOptions options, byte[] value) => new()
        {
            AbsoluteExpiration = options.AbsoluteExpiration,
            AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = options.SlidingExpiration,
            Size = value.Length
        };

        public void Set(string key, byte[]? value, DistributedCacheEntryOptions options)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (options is null) throw new ArgumentNullException(nameof(options));

            this._memCache.Set(key, new CacheItem(value), ParseOptions(options, value));
        }

        public Task SetAsync(string key, byte[]? value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            this.Set(key, value, options);
            return CompletedTask;
        }

        public void Refresh(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            this._memCache.TryGetValue(key, out _);
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            this.Refresh(key);
            return CompletedTask;
        }

        public void Remove(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            this._memCache.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            this.Remove(key);
            return CompletedTask;
        }

        public byte[]? HashGet(string key, string field)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (field is null) throw new ArgumentNullException(nameof(field));
            if (this._memCache.Get(key) is CacheItem item && item.Fields.TryGetValue(field, out var value)) return value;

            return null;
        }

        public Task<byte[]?> HashGetAsync(string key, string field, CancellationToken token = default)
        {
            return Task.FromResult(this.HashGet(key, field));
        }

        public void HashRemove(string key, string field)
        {
            if (this._memCache.Get(key) is CacheItem item) item.Fields.TryRemove(field, out _);
        }

        public Task HashRemoveAsync(string key, string field, CancellationToken token = default)
        {
            this.HashRemove(key, field);

            return CompletedTask;
        }

        public void HashSet(string key, string field, byte[]? value, DistributedCacheEntryOptions options)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (!(this._memCache.TryGetValue(key, out var v) && v is CacheItem item))
            {
                item = new CacheItem(new byte[] { 1 });
                this._memCache.Set(key, item, ParseOptions(options, item.Data));
            }

            item.Fields[field] = value;
        }

        public Task HashSetAsync(string key, string field, byte[]? value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            this.HashSet(key, field, value, options);
            return CompletedTask;
        }

        public void HashSetAll(string key, object? value, DistributedCacheEntryOptions options)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            Dictionary<string, byte[]> dict;
            if (value is System.Collections.IDictionary d)
            {
                dict = new Dictionary<string, byte[]>(d.Count);
                foreach (System.Collections.DictionaryEntry item in d)
                {
                    dict.Add(Convert.ToString(item.Key).MustBe(), item.Value?.Object2Bytes() ?? Array.Empty<byte>());
                }
            }
            else
            {
                var typeMapper = TypeMapper.Create(value.GetType());
                dict = new Dictionary<string, byte[]>(typeMapper.Count);
                foreach (var propertyMapper in typeMapper.Properties)
                {
                    dict.Add(propertyMapper.Name, propertyMapper.GetValue(value).Object2Bytes());
                }
            }

            foreach (var item in dict)
            {
                this.HashSet(key, item.Key, item.Value, options);
            }
        }

        public Task HashSetAllAsync(string key, object? value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            this.HashSetAll(key, value, options);
            return CompletedTask;
        }

        public T? HashGetAll<T>(string key)
        {
            if (this._memCache.TryGetValue(key, out var v) && v is CacheItem item)
            {
                var typeMapper = TypeMapper<T>.Instance;
                var t = (T?)Activator.CreateInstance(typeof(T), true);
                foreach (var propertyMapper in typeMapper.Properties)
                {
                    if (item.Fields.TryGetValue(propertyMapper.Name, out var value))
                    {
                        if (value != null)
                            propertyMapper.SetValue(t, value.Bytes2Object(propertyMapper.Property.PropertyType));
                    }
                }
                return t;
            }
            return default;
        }

        public Task<T?> HashGetAllAsync<T>(string key)
        {
            return Task.FromResult(this.HashGetAll<T>(key));
        }

        public T? HashGet<T>(string key, string field)
        {
            var obj = this.HashGet(key, field);
            if (obj == null)
                return default;
            return obj.Bytes2Object<T>();
        }

        public async Task<T?> HashGetAsync<T>(string key, string field, CancellationToken token = default)
        {
            var obj = await this.HashGetAsync(key, field, token);
            if (obj == null)
                return default;
            return obj.Bytes2Object<T>();
        }
    }
}
