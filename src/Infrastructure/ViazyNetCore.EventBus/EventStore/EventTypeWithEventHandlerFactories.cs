using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Handlers;

namespace ViazyNetCore.EventBus
{
    public class EventTypeWithEventHandlerFactories
    {
        public Type EventType { get; }

        public List<IEventHandlerFactory> EventHandlerFactories { get; }

        public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
        {
            EventType = eventType;
            EventHandlerFactories = eventHandlerFactories;
        }
    }
}
