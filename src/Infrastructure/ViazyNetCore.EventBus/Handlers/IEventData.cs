using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public interface IEventData
    {
        /// <summary>
        /// 事件发生的时间
        /// </summary>
        DateTime EventTime { get; set; }

        ///// <summary>
        ///// 触发事件的对象
        ///// </summary>
        //object EventSource { get; set; }
    }

    public class CommonEventData<TData> : IEventData
    {
        /// <summary>
        /// 事件发生的时间
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// 触发事件的对象
        /// </summary>
        public TData EventSource { get; set; }

        public CommonEventData()
        {
            this.EventTime = DateTime.Now;
        }
    }
}
