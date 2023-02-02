using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.ObjectPool;

namespace System.Web.DevServer
{
    /// <summary>
    /// Captures the completed-line notifications from a <see cref="EventedStreamReader"/>,
    /// combining the data into a single <see cref="string"/>.
    /// </summary>
    class EventedStreamStringReader : ObjectDisposableBase
    {
        private readonly static PooledObjectPolicy<StringBuilder> DefalutPolicy = new DefaultPooledObjectPolicy<StringBuilder>();
        private readonly static ObjectPool<StringBuilder> StringBuilderObjectPool = new DefaultObjectPool<StringBuilder>(DefalutPolicy);

        //最大可保留对象数量 = Environment.ProcessorCount * 2
        private readonly EventedStreamReader _eventedStreamReader;
        private StringBuilder _stringBuilder= StringBuilderObjectPool.Get();

        public EventedStreamStringReader(EventedStreamReader eventedStreamReader)
        {
            this._eventedStreamReader = eventedStreamReader ?? throw new ArgumentNullException(nameof(eventedStreamReader));
            this._eventedStreamReader.OnReceivedLine += this.OnReceivedLine;
        }

        public string ReadAsString() => this._stringBuilder.ToString();

        private void OnReceivedLine(string line) => this._stringBuilder.AppendLine(line);

        protected override void DisposeManaged()
        {
            this._eventedStreamReader.OnReceivedLine -= this.OnReceivedLine;
            StringBuilderObjectPool.Return(this._stringBuilder);
        }
    }
}
