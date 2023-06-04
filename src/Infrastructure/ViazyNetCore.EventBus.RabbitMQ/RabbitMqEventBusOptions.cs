using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.RabbitMQ;

namespace ViazyNetCore.EventBus.RabbitMQ
{
    public class RabbitMqEventBusOptions
    {
        public const string DefaultExchangeType = RabbitMqConsts.ExchangeTypes.Direct;

        public string ConnectionName { get; set; }

        public string ClientName { get; set; }

        public string ExchangeName { get; set; }

        public string ExchangeType { get; set; }

        public ushort? PrefetchCount { get; set; }

        public string GetExchangeTypeOrDefault()
        {
            return string.IsNullOrEmpty(ExchangeType)
                ? DefaultExchangeType
                : ExchangeType;
        }
    }
}
