using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore
{
    public class SerializerModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<ISerializer<string>, Serializer.JsonSerializer>();
            context.Services.AddSingleton<ISerializer<byte[]>, StringByteArraySerializer>();
        }
    }
}
