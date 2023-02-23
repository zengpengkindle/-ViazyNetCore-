namespace System.MQueue.Declares
{
    /// <summary>
    /// 定义一个队列的声明。
    /// </summary>
    public interface IQueueDeclare : IDeclare
    {
        /// <summary>
        /// 获取或设置一个值，表示是否自动删除。
        /// </summary>
        /// <remarks>
        /// <para>至少有一个消息者连接到这个队列，之后所有与这个队列连接的消息都断开时，才会自动删除。</para>
        /// <para>生产者客户端创建这个队列，或者没有消息者客户端连接这个队列时，不会自动删除这个队列。</para>
        /// </remarks>
        /// <value>若为 <see langword="true"/> 值时，当消息者连接断开连接后将删除队列。</value>
        bool AutoDelete { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示是否持久化。
        /// </summary>
        /// <value>若为 <see langword="true"/> 值时，队列在重新启动后继续有效。</value>
        bool Durable { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示是否排他队列。
        /// </summary>
        /// <value>若为 <see langword="true"/> 值时，该队列公对首次声明它的连接可见，并在连接断开时自动删除。</value>
        bool Exclusive { get; set; }

        QueueType QueueType { get; set; }
        ///// <summary>
        ///// 获取最后一次声明的结果。
        ///// </summary>
        ///// <value>如果当前</value>
        //QueueDeclareOk? LastDeclare { get; }
    }


    /// <summary>
    /// 表示一个队列的声明。
    /// </summary>
    public class QueueDeclare : DeclareBase, IQueueDeclare
    {
        /// <inheritdoc />
        public virtual bool Durable { get; set; }
        /// <inheritdoc />
        public virtual bool AutoDelete { get; set; }
        /// <inheritdoc />
        public virtual bool Exclusive { get; set; }

        public QueueType QueueType { get; set; } = QueueType.Classic;

        ///// <inheritdoc />
        //public QueueDeclareOk? LastDeclare { get; private set; }

        /// <summary>
        /// 初始化一个 <see cref="QueueDeclare"/> 类的新实例。
        /// </summary>
        /// <param name="queue">队列的名称。</param>
        public QueueDeclare(string queue) : base(queue) { }

        /// <inheritdoc />
        protected override void OnBuild(IChannelProxy channelProxy)
        {
            //this.LastDeclare =
            channelProxy.Channel?.QueueDeclare(this.Name, this.Durable, this.Exclusive, this.AutoDelete, this.Arguments);
        }

        /// <inheritdoc />
        protected override void OnBind(IChannelProxy channelProxy, IExchangeSource source)
        {
            channelProxy.Channel?.QueueBind(this.Name, source.Exchange.Name, source.RouterKey, source.Arguments);
        }
    }

    public enum QueueType
    {
        Classic = 1,
        Quorum = 2,
        Steam = 3
    }
}
