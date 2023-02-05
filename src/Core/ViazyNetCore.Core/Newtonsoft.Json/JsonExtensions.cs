using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
    public static class JsonExtensions
    {
        /// <summary>
        /// 初始化一个默认的 JSON 配置。
        /// </summary>
        /// <param name="serializerSettings">JSON 序列化配置。</param>
        /// <returns>JSON 序列化配置。</returns>
        public static JsonSerializerSettings InitializeDefault(this JsonSerializerSettings serializerSettings)
        {
            if (serializerSettings is null)
            {
                throw new ArgumentNullException(nameof(serializerSettings));
            }

            serializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            //serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
            //蛇形：，new Serialization.SnakeCaseNamingStrategy
            serializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(),
            }; ;
            serializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            //serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.Converters = new List<JsonConverter>() { new LongJsonConverter(),new NullableEnumConverter() };
            serializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            serializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            return serializerSettings;
        }
    }
}
