﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ViazyNetCore.Serializer
{
    public class JsonSerializer : ISerializer<string>
    {
        #region Implementation of ISerializer<string>

        /// <summary>
        /// 序列化。
        /// </summary>
        /// <param name="instance">需要序列化的对象。</param>
        /// <returns>序列化之后的结果。</returns>
        public string Serialize(object instance)
        {
            return JSON.Stringify(instance);
        }

        /// <summary>
        /// 反序列化。
        /// </summary>
        /// <param name="content">序列化的内容。</param>
        /// <param name="type">对象类型。</param>
        /// <returns>一个对象实例。</returns>
        public object Deserialize(string content, Type type)
        {
            return JSON.Parse(content, type);
        }

        public object Deserialize(byte[] value, Type type)
        {
            return JSON.Deserialize(value, type);
        }

        #endregion Implementation of ISerializer<string>
    }
}
