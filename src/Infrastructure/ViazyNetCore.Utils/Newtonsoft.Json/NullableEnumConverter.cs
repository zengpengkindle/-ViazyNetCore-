using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace Newtonsoft.Json
{
    public class NullableEnumConverter : JsonConverter
    {

        private Type _type;

        /// <summary>
        /// 是否可以转换
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            this._type = objectType;
            return (objectType.IsNullable() ? Nullable.GetUnderlyingType(objectType) : objectType).IsEnum;
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType.IsNullable())
                    return null;
                else
                    throw new JsonSerializationException($"Cannot convert null value to {objectType.FullName}.");
            }
            if (reader.Value is string stv)
            {
                if (stv.Length == 0)
                {
                    if (objectType.IsEnum) return null;
                    return null;
                }
            }

            object obj = null;

            bool flag = objectType.IsNullable();
            Type enumType = (flag ? Nullable.GetUnderlyingType(objectType) : objectType);
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            try
            {
                obj = Convert.ChangeType(reader.Value, underlyingType, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }
            if (obj != null)
            {
                var result = Enum.ToObject(enumType, obj);
                if (Enum.IsDefined(enumType, obj)) return result;
                if (result.Equals(0)) return null;// 非默认值，为0时则返回null，否则返回“未知”值
                return result;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null) writer.WriteNull();
            else
            {
                //writer.WriteValue((int)value);
                writer.WriteValue(Convert.ToInt32(value));
            }
        }
    }
}
