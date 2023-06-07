using System.Linq;
using System.MQueue.Declares;
using ViazyNetCore;

namespace System.MQueue
{
    /// <summary>
    /// 定义一个消息。
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 获取消息编号。
        /// </summary>
        string Id { get; }
        /// <summary>
        /// 获取或设置一个值，表示连接名称。
        /// </summary>
        string? ConnectionName { get; set; }
        /// <summary>
        /// 获取交换机名称。
        /// </summary>
        string Exchange { get; }
        /// <summary>
        /// 获取队列名称。
        /// </summary>
        string Queue { get; }
        /// <summary>
        /// 生成队列。
        /// </summary>
        /// <param name="declareFactory">声明工厂。</param>
        /// <returns>队列。</returns>
        IQueueDeclare Build(IDeclareFactory declareFactory);
    }

    /// <summary>
    /// 表示一个类型化的消息。
    /// </summary>
    public class TypedMessage : IMessage
    {
        private readonly IMessageBinder[] _messageBinders;

        /// <inheritdoc />
        public string Id { get; }
        /// <inheritdoc />
        public string? ConnectionName { get; set; }
        /// <inheritdoc />
        public string Exchange { get; }
        /// <inheritdoc />
        public string Queue { get; }

        /// <summary>
        /// 获取消息内容的类型。
        /// </summary>
        public Type BodyType { get; }

        /// <summary>
        /// 初始化一个 <see cref="TypedMessage"/> 类的新实例。
        /// </summary>
        /// <param name="bodyType">消息的实体类型。</param>
        public TypedMessage(Type bodyType)
        {
            this.BodyType = bodyType ?? throw new ArgumentNullException(nameof(bodyType));
            this._messageBinders = bodyType.GetAttributes<IMessageBinder>().OrderBy(f => f.Order).ToArray();

            var ma = bodyType.GetAttribute<MessageAttribute>();
            string? id = null, queue = null, exchange = null;

            if (ma is not null)
            {
                id = ma.Id;
                queue = ma.Queue;
                exchange = ma.Exchange;
            }

            var fullName = bodyType.FullName.MustBe();
            id = id.IsNull() ? fullName : id;
            this.Id = id;
            this.Queue = queue.IsNull() ? id + ".Queue" : queue;
            this.Exchange = exchange.IsNull() ? id + ".Exchange" : exchange;
        }

        private IQueueDeclare? _queue;

        private static object _lockObject = new();

        /// <inheritdoc />
        public IQueueDeclare Build(IDeclareFactory declareFactory)
        {
            if (declareFactory is null)
            {
                throw new ArgumentNullException(nameof(declareFactory));
            }

            if (this._queue is not null) return this._queue;

            lock (_lockObject)
            {
                if (this._queue is not null) return this._queue;

                var queue = declareFactory.Queue(this.Queue);

                foreach (var binder in this._messageBinders)
                {
                    queue = binder.Bind(declareFactory, this, queue);
                }

                if (queue.Sources.Count == 0)
                {
                    queue.Sources.Add(new ExchangeSource(null, declareFactory.Exchange(this.Exchange), null));
                }

                this._queue = queue;
                return queue;
            }
        }
    }
}
