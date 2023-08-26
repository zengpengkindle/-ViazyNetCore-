using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.EventBus.Distributed
{
    public class DistributedEventBus : DefaultEventBus
    {
        private readonly IDistributedEventStore _distributedEventStore;

        public DistributedEventBus(ILocalEventStore localEventStore
            , IDistributedEventStore distributedEventStore
            , IServiceProvider serviceProvider
            , IEventHandlerInvoker eventHandlerInvoker) : base(localEventStore, serviceProvider, eventHandlerInvoker)
        {
            this._distributedEventStore = distributedEventStore;
        }

        public override async Task PublishAsync(Type eventType, object eventData)
        {
            await base.PublishAsync(eventType, eventData);
            if (eventType.GetCustomAttribute<MessageAttribute>() != null)
                await this._distributedEventStore.PublishAsync(eventType, eventData);
        }
    }
}
