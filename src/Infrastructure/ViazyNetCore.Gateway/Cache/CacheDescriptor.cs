﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Gateway.Cache
{
    public class CacheDescriptor
    {
        /// <summary>
        /// 初始化一个新的服务描述符。
        /// </summary>
        public CacheDescriptor()
        {
            Metadatas = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 缓存Id。
        /// </summary>
        public string Id { get; set; }

        public string Prefix { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 元数据。
        /// </summary>
        public IDictionary<string, object> Metadatas { get; set; }

        /// <summary>
        /// 获取一个元数据。
        /// </summary>
        /// <typeparam name="T">元数据类型。</typeparam>
        /// <param name="name">元数据名称。</param>
        /// <param name="def">如果指定名称的元数据不存在则返回这个参数。</param>
        /// <returns>元数据值。</returns>
        public T GetMetadata<T>(string name, T def = default(T))
        {
            if (!Metadatas.ContainsKey(name))
                return def;

            return (T)Metadatas[name];
        }

        #region Equality members

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            var model = obj as CacheDescriptor;
            if (model == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            if (model.Id != Id)
                return false;

            return model.Metadatas.Count == Metadatas.Count && model.Metadatas.All(metadata =>
            {
                object value;
                if (!Metadatas.TryGetValue(metadata.Key, out value))
                    return false;

                if (metadata.Value == null && value == null)
                    return true;
                if (metadata.Value == null || value == null)
                    return false;

                return metadata.Value.Equals(value);
            });
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(CacheDescriptor model1, CacheDescriptor model2)
        {
            return Equals(model1, model2);
        }

        public static bool operator !=(CacheDescriptor model1, CacheDescriptor model2)
        {
            return !Equals(model1, model2);
        }

        #endregion Equality members
    }
}
