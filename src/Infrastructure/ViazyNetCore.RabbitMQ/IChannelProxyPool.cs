namespace System.MQueue
{
    /// <summary>
    /// 定义一个通道代理池。
    /// </summary>
    public interface IChannelProxyPool
    {
        /// <summary>
        /// 获取消息管理器。
        /// </summary>
        IMessageManager Manager { get; }
        /// <summary>
        /// 获取通道代理。
        /// </summary>
        /// <param name="connectionName">连接名称。</param>
        /// <param name="message">消息。</param>
        /// <returns>通道代理。</returns>
        IChannelProxy GetChannel(string? connectionName, IMessage message);
    }

    class DefaultIChannelProxyPool : IChannelProxyPool
    {
        private readonly IDeclareFactory _declareFactory;

        public DefaultIChannelProxyPool(IMessageManager messageManager, IDeclareFactory declareFactory)
        {
            this.Manager = messageManager;
            this._declareFactory = declareFactory;
        }

        public IMessageManager Manager { get; }

        public IChannelProxy GetChannel(string? connectionName, IMessage message)
        {
            if(message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var channelProxy = this.TryGetChannel(connectionName, message, 0);
            if(channelProxy is null) throw new InvalidOperationException("The message quene connection is disconnected when try more times connecting.");
            return channelProxy;
        }

        private IChannelProxy? TryGetChannel(string? connectionName, IMessage message, int testCount)
        {
            if(testCount > this.Manager.Options.ChannelCreateRetryCount) return null;

            var connectionProxyFactory = this.Manager.Get(connectionName ?? message.ConnectionName);
            if(connectionProxyFactory is null) throw new InvalidOperationException($"Cannot found connection name '{message.ConnectionName}' factory.");
            var connectionProxy = connectionProxyFactory.Create();

            if(!connectionProxy.IsOpen) return null;

            var channelProxy = connectionProxy.GetChannel(false);
            if(channelProxy is null)
            {
                return this.TryGetChannel(connectionName, message, testCount + 1);
            }

            var queue = message.Build(this._declareFactory);

            if(!connectionProxy.IsDeclareBuild(message))
            {
                queue.Build(channelProxy);
            }

            return channelProxy;
        }
    }
}