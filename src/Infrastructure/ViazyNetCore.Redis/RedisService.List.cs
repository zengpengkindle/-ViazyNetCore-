using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace ViazyNetCore.Redis
{
    public partial class RedisService : IRedisListCache
    {
        /// <summary>
        /// 根据key获取RedisValue
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<RedisValue[]> ListRangeAsync(string redisKey)
        {
            return await this.GetDatabase().ListRangeAsync(redisKey);
        }

        public async Task<List<T?>> ListRangeAsync<T>(string redisKey)
        {
            return (await this.ListRangeAsync(redisKey)).Select(p => JsonConvert.DeserializeObject<T>(p!)).ToList();
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1)
        {
            var result = await this.GetDatabase().ListRangeAsync(redisKey);
            return result.Select(o => o.ToString());
        }

        /// <summary>
        /// 根据索引获取指定位置数据
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop)
        {
            var result = await this.GetDatabase().ListRangeAsync(redisKey, start, stop);
            return result.Select(o => o.ToString());
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync(string redisKey, string redisValue)
        {
            return await this.GetDatabase().ListLeftPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string redisKey, string redisValue)
        {
            return await this.GetDatabase().ListRightPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入数组集合。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue)
        {
            var redislist = new List<RedisValue>();
            foreach (var item in redisValue)
            {
                redislist.Add(item);
            }
            return await this.GetDatabase().ListRightPushAsync(redisKey, redislist.ToArray());
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素  反序列化
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T?> ListLeftPopAsync<T>(string redisKey) where T : class
        {
            var cacheValue = await this.GetDatabase().ListLeftPopAsync(redisKey);
            if (string.IsNullOrEmpty(cacheValue)) return null;
            var res = JsonConvert.DeserializeObject<T>(cacheValue!);
            return res;
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素   反序列化
        /// 只能是对象集合
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T?> ListRightPopAsync<T>(string redisKey) where T : class
        {
            var cacheValue = await this.GetDatabase().ListRightPopAsync(redisKey);
            if (string.IsNullOrEmpty(cacheValue)) return null;
            var res = JsonConvert.DeserializeObject<T>(cacheValue!);
            return res;
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素   
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string?> ListLeftPopAsync(string redisKey)
        {
            return await this.GetDatabase().ListLeftPopAsync(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素   
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string?> ListRightPopAsync(string redisKey)
        {
            return await this.GetDatabase().ListRightPopAsync(redisKey);
        }

        /// <summary>
        /// 列表长度
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string redisKey)
        {
            return await this.GetDatabase().ListLengthAsync(redisKey);
        }

        /// <summary>
        /// 删除List中的元素 并返回删除的个数
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">元素</param>
        /// <param name="type">大于零 : 从表头开始向表尾搜索，小于零 : 从表尾开始向表头搜索，等于零：移除表中所有与 VALUE 相等的值</param>
        /// <returns></returns>
        public async Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0)
        {
            return await this.GetDatabase().ListRemoveAsync(redisKey, redisValue, type);
        }

        /// <summary>
        /// 清空List
        /// </summary>
        /// <param name="redisKey"></param>
        public async Task ListClearAsync(string redisKey)
        {
            await this.GetDatabase().ListTrimAsync(redisKey, 1, 0);
        }

        /// <summary>
        /// 有序集合/定时任务延迟队列用的多
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">元素</param>
        /// <param name="score">分数</param>
        public async Task SortedSetAddAsync(string redisKey, string redisValue, double score)
        {
            await this.GetDatabase().SortedSetAddAsync(redisKey, redisValue, score);
        }
    }
}
