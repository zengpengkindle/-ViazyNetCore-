using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.EventBus.Distributed
{
    public class DistributedEventBus : DefaultEventBus
    {
        private readonly IDistributedEventStore _distributedEventStore;

        public DistributedEventBus(ILocalEventStore localEventStore
            , IDistributedEventStore distributedEventStore
            , IEventHandlerInvoker eventHandlerInvoker) : base(localEventStore, eventHandlerInvoker)
        {
            this._distributedEventStore = distributedEventStore;
        }

        public override async Task PublishAsync<TEventData>(Type eventHandlerType, TEventData @event)
        {
            await base.PublishAsync(eventHandlerType, @event);
            if (eventHandlerType.GetCustomAttribute<MessageAttribute>() != null)
                await this._distributedEventStore.PublishAsync(eventHandlerType, @event);
        }
    }
}
