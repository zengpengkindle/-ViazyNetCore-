using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Dapr
{
    public interface IDaprSerializer
    {
        byte[] Serialize(object obj);

        object Deserialize(byte[] value, Type type);

        object Deserialize(string value, Type type);
    }
}
