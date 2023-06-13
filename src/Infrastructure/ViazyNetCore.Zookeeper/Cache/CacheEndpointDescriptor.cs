﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Zookeeper.Cache
{
    /// <summary>
    /// 服务地址描述符。
    /// </summary>
    public class CacheEndpointDescriptor
    {
        /// <summary>
        /// 地址类型。
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 地址值。
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 创建一个描述符。
        /// </summary>
        /// <typeparam name="T">地址模型类型。</typeparam>
        /// <param name="address">地址模型实例。</param>
        /// <param name="serializer">序列化器。</param>
        /// <returns>服务地址描述符。</returns>
        public static CacheEndpointDescriptor CreateDescriptor<T>(T address, ISerializer<string> serializer) where T : CacheEndpoint, new()
        {
            return new CacheEndpointDescriptor
            {
                Type = typeof(T).FullName,
                Value = serializer.Serialize(address)
            };
        }
    }
}
