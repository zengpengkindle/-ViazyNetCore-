using System.Collections.Concurrent;

namespace System.MQueue
{
    /// <summary>
    /// 定义一个消息工厂。
    /// </summary>
    public interface IMessageFactory
    {
        /// <summary>
        /// 获取指定编号的消息。
        /// </summary>
        /// <param name="id">消息编号。</param>
        /// <returns>消息。</returns>
        IMessage? Get(string id);
        /// <summary>
        /// 设置消息。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <returns>消息工厂。</returns>
        IMessageFactory Set(IMessage message);

        /// <summary>
        /// 获取指定实体的消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <returns>消息。</returns>
        IMessage Get<TBody>();

        /// <summary>
        /// 获取指定实体的消息。
        /// </summary>
        /// <typeparam name="type">实体的数据类型。</typeparam>
        /// <returns>消息。</returns>
        IMessage Get(Type type);
    }

    class DefaultMessageFactory : IMessageFactory
    {
        protected ConcurrentDictionary<string, IMessage> Messages { get; }

        public DefaultMessageFactory()
        {
            this.Messages = new ConcurrentDictionary<string, IMessage>(StringComparer.OrdinalIgnoreCase);
        }

        public virtual IMessageFactory Set(IMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.Messages[message.Id] = message;
            return this;
        }

        public virtual IMessage? Get(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return this.Messages.TryGetValue(id, out var message) ? message : null;
        }

        public virtual IMessage Get<TBody>()
        {
            return this.Messages.GetOrAdd("Typed#" + typeof(TBody).FullName, key => new TypedMessage(typeof(TBody)));
        }

        public virtual IMessage Get(Type type)
        {
            return this.Messages.GetOrAdd("Typed#" + type.FullName, key => new TypedMessage(type));
        }
    }

}
