using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Dapr
{
    [Injection]
    public class Utf8JsonDaprSerializer : IDaprSerializer
    {
        private readonly ISerializer<string> _jsonSerializer;

        public Utf8JsonDaprSerializer(ISerializer<string> jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }
        public object Deserialize(byte[] value, Type type)
        {
            return _jsonSerializer.Deserialize(value, type);
        }

        public object Deserialize(string value, Type type)
        {
            return _jsonSerializer.Deserialize(value, type);
        }

        public byte[] Serialize(object obj)
        {
            return Encoding.UTF8.GetBytes(_jsonSerializer.Serialize(obj));
        }
    }
}
