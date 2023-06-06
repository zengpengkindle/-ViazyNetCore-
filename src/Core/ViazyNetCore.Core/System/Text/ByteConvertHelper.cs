using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace System
{
    public static class ByteConvertHelper
    {
        private static readonly JsonSerializerOptions DEFAULT_JsonSerializerOptions = new()
        {
            WriteIndented = false,
        };
        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        public static byte[] Object2Bytes(this object obj)
        {
            return JSON.Serialize(obj);
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static T? Bytes2Object<T>(this byte[] buff)
        {
            return JSON.Deserialize<T>(buff);
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static object? Bytes2Object(this byte[] buff, Type type)
        {
            return JSON.Deserialize(buff, type);
        }
    }
}
