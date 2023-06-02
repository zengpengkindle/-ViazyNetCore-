using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Handlers;

namespace ViazyNetCore
{
    public class EventHandlerFactoryUnregistrar : IDisposable
    {
        private readonly IEventStore _eventStore;
        private readonly Type _eventType;
        private readonly IEventHandlerFactory _factory;

        public EventHandlerFactoryUnregistrar(IEventStore eventStore, Type eventType, IEventHandlerFactory factory)
        {
            this._eventStore = eventStore;
            _eventType = eventType;
            _factory = factory;
        }

        public void Dispose()
        {
            _eventStore.Unsubscribe(_eventType, _factory);
        }
    }
}
