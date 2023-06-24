using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ViazyNetCore.Dapr
{
    public class SystemTextJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; }

        public SystemTextJsonSerializerOptions()
        {
            JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };
        }
    }
}
