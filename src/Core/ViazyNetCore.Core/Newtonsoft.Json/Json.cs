using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace System
{
    /// <summary>
    /// 表示通用 JSON 标准库。
    /// </summary>
    public static class JSON
    {
        internal readonly static Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver NamesContractResolver
            = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            {
                NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy()
            };
        /// <summary>
        /// 默认的序列化配置。
        /// </summary>
        public static JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings().InitializeDefault();

        /// <summary>
        /// 转化 JSON 格式化的名称。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <returns>转换后的名称。</returns>
        public static string ResolvedName(string name) => NamesContractResolver.GetResolvedPropertyName(name);

        /// <summary>
        /// 将值转换为 JSON 字符串。
        /// </summary>
        /// <param name="value">要转换的值。</param>
        /// <param name="indented">是否包含缩进。</param>
        /// <param name="serializerSettings">序列化配置。</param>
        /// <returns>一个 JSON 字符串。</returns>
        public static string Stringify(object value, bool indented = false, JsonSerializerSettings? serializerSettings = null)
        {
            return JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, serializerSettings ?? SerializerSettings);
        }

        /// <summary>
        /// 解析 JSON 字符串，指定类型 <paramref name="type"/> 构造由字符串描述的值或对象。
        /// </summary>
        /// <param name="text">要被解析成 JavaScript 值的字符串。</param>
        /// <param name="type">对象的数据类型。</param>
        /// <param name="serializerSettings">序列化配置。</param>
        /// <returns>对应给定 JSON 文本的对象/值。</returns>
        public static object? Parse(string text, Type type, JsonSerializerSettings? serializerSettings = null)
        {
            return JsonConvert.DeserializeObject(text, type, serializerSettings ?? SerializerSettings);
        }
        /// <summary>
        /// 将值序列化。
        /// </summary>
        /// <param name="value">要转换的值。</param>
        /// <param name="indented">是否包含缩进。</param>
        /// <param name="serializerSettings">序列化配置。</param>
        /// <returns>一个 JSON 字符串。</returns>
        public static byte[] Serialize(object value, bool indented = false, JsonSerializerSettings? serializerSettings = null)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, serializerSettings ?? SerializerSettings));
        }

        /// <summary>
        /// 解析 JSON 字符串，指定类型 <typeparamref name="T"/> 构造由字符串描述的值或对象。
        /// </summary>
        /// <typeparam name="T">对象的数据类型。</typeparam>
        /// <param name="text">要被解析成 JavaScript 值的字符串。</param>
        /// <param name="serializerSettings">序列化配置。</param>
        /// <returns>对应给定 JSON 文本的对象/值。</returns>
        public static T? Parse<T>(string text, JsonSerializerSettings? serializerSettings = null)
        {
            return JsonConvert.DeserializeObject<T>(text, serializerSettings ?? SerializerSettings);
        }

        /// <summary>
        /// 解析 JSON 字符串。
        /// </summary>
        /// <param name="text">要被解析成 JavaScript 值的字符串。</param>
        public static JObject Parse(string text)
        {
            return JObject.Parse(text);
        }

        /// <summary>
        /// 将 JSON 元素转换为指定类型的值。
        /// </summary>
        /// <typeparam name="T">转换的类型。</typeparam>
        /// <param name="element">JSON 元素。</param>
        /// <returns>转换后的值。</returns>
        public static T? ToObject<T>(this JObject element)
        {
            if(element is null) throw new ArgumentNullException(nameof(element));
            return element.ToObject<T>();
        }

        /// <summary>
        /// 将 JSON 元素转换为指定类型的值。
        /// </summary>
        /// <param name="element">JSON 元素。</param>
        /// <param name="type">转换的类型。</param>
        /// <returns>转换后的值。</returns>
        public static object? ToObject(this JObject element, Type type)
        {
            if(element is null) throw new ArgumentNullException(nameof(element));
            if(type is null) throw new ArgumentNullException(nameof(type));
            return element.ToObject(type);
        }
    }
}