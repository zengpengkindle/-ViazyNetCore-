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
    public partial class RedisService
    {
        public List<T?> GetAllItemsFromList<T>(string key, int? db = null)
        {
            return this._database.HashGetAll(key).Select(p => JSON.Parse<T>(p.Value!)).ToList();
        }

        public List<T?> GetRangeFromList<T>(string key, int startingFrom, int endingAt, int? db = null)
        {
            return this._database.ListRange(key, startingFrom, endingAt).Select(p => JSON.Parse<T>(p!)).ToList();
        }

        public void AddItemToList<T>(string listId, T value, int? db = null)
        {
            this._database.SetAdd(listId, JSON.Stringify(value));
        }
    }
}
