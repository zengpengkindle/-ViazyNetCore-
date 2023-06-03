using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public interface IEventHandlerInvoker
    {
        Task InvokeAsync(IEventHandler eventHandler, object eventData, Type eventType);
    }
}
