using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
    public class LongJsonConverter : JsonConverter
    {
        /// <summary>
        /// 是否可以转换
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(long) || objectType == typeof(long?);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType == typeof(long)) return 0L;
                return null;
            }
            if (reader.Value is string stv)
            {
                if (stv.Length == 0)
                {
                    if (objectType == typeof(long)) return 0L;
                    return null;
                }
                return long.Parse(stv);
            }
            return Convert.ChangeType(reader.Value, typeof(long));// objectType); ;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null) writer.WriteNull();
            else
            {
                if (value is long lv && lv <= int.MaxValue) writer.WriteValue((int)lv);
                else writer.WriteValue(value.ToString());
            }
        }
    }

    class GuidConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid) || objectType == typeof(Guid?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType == typeof(Guid)) return Guid.Empty;
                return null;
            }
            if (reader.Value is string stv)
            {
                if (stv.Length == 0)
                {
                    if (objectType == typeof(Guid)) return Guid.Empty;
                    return null;
                }
                return Guid.Parse(stv);
            }

            return Convert.ChangeType(reader.Value, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null) writer.WriteNull();
            else if (value is Guid g)
            {
                if (g == Guid.Empty) writer.WriteValue(string.Empty);
                else writer.WriteValue(g.ToString("N"));
            }
        }
    }
}