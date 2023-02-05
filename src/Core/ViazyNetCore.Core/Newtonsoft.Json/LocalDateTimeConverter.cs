using System;
using System.Collections.Generic;
using System.Text;

namespace Newtonsoft.Json
{
    class LocalDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            if(value is null) return null;
            if(value is DateTime dv) return dv.ToLocalTime();
            if(value is long lv) return lv.ConvertFromJsTime().ToLocalTime();
            if(value is string sv) return DateTime.Parse(sv).ToLocalTime();
            return Convert.ToDateTime(value).ToLocalTime();

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(value is null) writer.WriteNull();
            else
            {
                var dateTime = (DateTime)value;
                writer.WriteValue(dateTime.ToUniversalTime());
            }
        }
    }
}
