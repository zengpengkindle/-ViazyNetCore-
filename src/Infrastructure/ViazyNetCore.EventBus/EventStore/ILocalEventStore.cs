using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public interface ILocalEventStore : IEventStore
    {
        IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler)
    where TEvent : IEventData;
    }
}
