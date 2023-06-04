using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.RabbitMQ
{
    public class RabbitMqConsts
    {
        public static class DeliveryModes
        {
            public const int NonPersistent = 1;

            public const int Persistent = 2;
        }

        public static class ExchangeTypes
        {
            public const string Direct = "direct";

            public const string Topic = "topic";

            public const string Fanout = "fanout";

            public const string Headers = "headers";
        }
    }
}
