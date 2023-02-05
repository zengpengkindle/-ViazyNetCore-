using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace ViazyNetCore.Caching
{
    public interface IDistributedHashCache : IDistributedCache
    {
        /// <summary>
        /// 从哈希表中获取值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="field">字段名。</param>
        /// <returns>值。</returns>
        byte[]? HashGet(string key, string field);
        /// <summary>
        /// 异步从哈希表中获取值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="field">字段名。</param>
        /// <param name="token">取消标识。</param>
        /// <returns>值。</returns>
        Task<byte[]?> HashGetAsync(string key, string field, CancellationToken token = default);
        /// <summary>
        /// 从哈希表中获取值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="field">字段名。</param>
        /// <returns>值。</returns>
        T? HashGet<T>(string key, string field);
        /// <summary>
        /// 异步从哈希表中获取值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="field">字段名。</param>
        /// <param name="token">取消标识。</param>
        /// <returns>值。</returns>
        Task<T?> HashGetAsync<T>(string key, string field, CancellationToken token = default);
        /// <summary>
        /// 从哈希表中移除字段。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="field">字段名。</param>
        void HashRemove(string key, string field);
        /// <summary>
        /// 异步从哈希表中移除字段。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="field">字段名。</param>
        /// <param name="token">取消标识。</param>
        /// <returns>异步任务。</returns>
        Task HashRemoveAsync(string key, string field, CancellationToken token = default);
        /// <summary>
        /// 从哈希表中设置值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="field">字段名。</param>
        /// <param name="options">缓存配置（若键不存在时生效，存在则不做任何动作）。</param>
        /// <param name="value">值。</param>
        void HashSet(string key, string field, byte[]? value, DistributedCacheEntryOptions options);
        /// <summary>
        /// 异步哈希表中设置值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="field">字段名。</param>
        /// <param name="value">值。</param>
        /// <param name="options">缓存配置（若键不存在时生效，存在则不做任何动作）。</param>
        /// <param name="token">取消标识。</param>
        /// <returns>异步任务。</returns>
        Task HashSetAsync(string key, string field, byte[]? value, DistributedCacheEntryOptions options, CancellationToken token = default);

        /// <summary>
        /// 哈希表中批量设置值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="options">缓存配置（若键不存在时生效，存在则不做任何动作）。</param>
        /// <param name="value">值。</param>
        void HashSetAll(string key, object? value, DistributedCacheEntryOptions options);

        /// <summary>
        /// 异步哈希表中批量设置值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="options">缓存配置（若键不存在时生效，存在则不做任何动作）。</param>
        /// <param name="value">值。</param>
        /// <param name="token">取消标识。</param>
        /// <returns>异步任务。</returns>
        Task HashSetAllAsync(string key, object? value, DistributedCacheEntryOptions options, CancellationToken token = default);
        /// <summary>
        /// 批量从哈希表中获取值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键名。</param>
        /// <returns>值。</returns>
        T? HashGetAll<T>(string key);
        /// <summary>
        /// 批量异步从哈希表中获取值。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="key">键名。</param>
        /// <returns>值。</returns>
        Task<T?> HashGetAllAsync<T>(string key);
    }
}
