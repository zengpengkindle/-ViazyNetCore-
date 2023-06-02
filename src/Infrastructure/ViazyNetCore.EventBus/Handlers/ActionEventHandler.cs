using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Handlers
{
    /// <summary>
    /// 支持Action的事件处理器
    /// </summary>
    /// <typeparam name="TEventData"></typeparam>
    public class ActionEventHandlerAsync<TEvent> : IEventHandlerAsync<TEvent> where TEvent : IEventData
    {
        /// <summary>
        /// 定义Action的引用，并通过构造函数传参初始化
        /// </summary>
        public Func<TEvent, Task> Action { get; }

        public ActionEventHandlerAsync(Func<TEvent, Task> handler)
        {
            this.Action = handler;
        }

        /// <summary>
        /// 调用具体的Action来处理事件逻辑
        /// </summary>
        /// <param name="eventData"></param>
        public Task HandleEventAsync(TEvent eventData)
        {
            this.Action(eventData);
            return Task.CompletedTask;
        }
    }
}
