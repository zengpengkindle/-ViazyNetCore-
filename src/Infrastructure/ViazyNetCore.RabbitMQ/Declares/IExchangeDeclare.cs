
using RabbitMQ.Client;

namespace System.MQueue.Declares
{
    /// <summary>
    /// 定义一个交换机声明。
    /// </summary>
    public interface IExchangeDeclare : IDeclare
    {
        /// <summary>
        /// 获取或设置交换机类型。
        /// </summary>
        string? Type { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示自动删除。
        /// </summary>
        /// <value>若为 <see langword="true"/> 值时，当最后一个队列与之解除绑定时将删除交换机。</value>
        bool AutoDelete { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示是否持久化。
        /// </summary>
        /// <value>若为 <see langword="true"/> 值时，交换机在重新启动后继续有效。</value>
        bool Durable { get; set; }
    }

    /// <summary>
    /// 表示一个交换机声明。
    /// </summary>
    public class ExchangeDeclare : DeclareBase, IExchangeDeclare
    {
        /// <inheritdoc />
        public virtual string? Type { get; set; }
        /// <inheritdoc />
        public virtual bool Durable { get; set; }
        /// <inheritdoc />
        public virtual bool AutoDelete { get; set; }

        /// <summary>
        /// 初始化一个 <see cref="ExchangeDeclare"/> 类的新实例。
        /// </summary>
        /// <param name="exchange">交换机的名称。</param>
        public ExchangeDeclare(string exchange) : base(exchange) { }

        /// <inheritdoc />
        protected override void OnBuild(IChannelProxy channelProxy)
        {
            channelProxy.Channel?.ExchangeDeclare(this.Name, this.Type ?? ExchangeType.Direct, this.Durable, this.AutoDelete, this.Arguments);
        }

        /// <inheritdoc />
        protected override void OnBind(IChannelProxy channelProxy, IExchangeSource source)
        {
            channelProxy.Channel?.ExchangeBind(this.Name, source.Exchange.Name, source.RouterKey, source.Arguments);
        }
    }
}
