using System.Threading;

namespace System.MQueue
{
    /// <summary>
    /// 定义一个消息总线。
    /// </summary>
    public interface IMessageBus : IMessageBusCore
    {
        /// <summary>
        /// 创建或获取消息总线的上下文。
        /// </summary>
        IMessageBusContext Context { get; }
        /// <summary>
        /// 创建或获取带事务的消息总线的上下文。
        /// </summary>
        IMessageBusContext ContextTransaction { get; }
        /// <summary>
        /// 获取一个值，表示消息总线的上下文是否已创建。
        /// </summary>
        bool IsContextCreated { get; }
    }

    class DefaultMessageBus : DefaultMessageBusBase, IMessageBus
    {

        private readonly Local<IMessageBusContext> _threadLocalContent;

        public DefaultMessageBus(IChannelProxyPool channelPool
            , IMessageFactory messageFactory
            , IMessageSerializer messageSerializer)
            : base(channelPool, messageFactory, messageSerializer)
        {
            this._threadLocalContent = new Local<IMessageBusContext>(() => new DefaultMessageBusContext(this, channelPool, messageFactory, messageSerializer));
        }

        [System.Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        public IMessageBusContext Context => this._threadLocalContent.Value;

        [System.Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        public IMessageBusContext ContextTransaction => this._threadLocalContent.Value.OpenTransaction();

        public bool IsContextCreated => this._threadLocalContent.IsValueCreated;

        public override TTask CallAsync<TTask>(IMessage message, Func<IChannelProxy, IMessage, TTask> channelAction, string? connectionName)
        {
            if(message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if(channelAction is null)
            {
                throw new ArgumentNullException(nameof(channelAction));
            }

            var channelProxy = this._channelPool.GetChannel(connectionName, message);

            try
            {
                return channelAction(channelProxy, message);
            }
            catch(Exception ex)
            {
                this.Manager.EmitError(this, ex);
                throw;
            }
            finally
            {
                channelProxy.TryFree();
            }
        }

        internal void ResetContext() => this._threadLocalContent.Reset(false);

    }

}
