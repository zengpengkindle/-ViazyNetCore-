namespace ViazyNetCore.RabbitMQ
{
    /// <summary>
    /// 定义一个消息总线上下文。
    /// </summary>
    public interface IMessageBusContext : IMessageBusCore, IDisposable
    {
        /// <summary>
        /// （幂等操作）打开事务。
        /// </summary>
        /// <returns>消息总线上下文。</returns>
        IMessageBusContext OpenTransaction();
        /// <summary>
        /// 提交事务。
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务。
        /// </summary>
        void Rollback();
    }

    class DefaultMessageBusContext : DefaultMessageBusBase, IMessageBusContext
    {
        private readonly object _syncObject;
        private Lazy<IChannelProxy>? _lazyChannelProxy;
        private readonly DefaultMessageBus _owner;
        private bool _isTransaction;
        private bool _autoRollback;
        private bool _isSubmited;

        public DefaultMessageBusContext(DefaultMessageBus owner, IChannelProxyPool channelPool
            , IMessageFactory messageFactory
            , IMessageSerializer messageSerializer)
            : base(channelPool, messageFactory, messageSerializer)
        {
            this._syncObject = new object();
            this._owner = owner;
        }


        public IMessageBusContext OpenTransaction()
        {
            this._isTransaction = true;
            this._autoRollback = true;
            return this;
        }

        public void Commit()
        {
            if (this._isTransaction && !this._isSubmited && this._lazyChannelProxy is not null)
            {
                this._autoRollback = false;
                this._isSubmited = true;
                this._lazyChannelProxy.Value.Channel?.TxCommit();
            }
        }

        public void Rollback()
        {
            if (this._isTransaction && !this._isSubmited && this._lazyChannelProxy is not null)
            {
                this._autoRollback = false;
                this._isSubmited = true;
                this._lazyChannelProxy.Value.Channel?.TxRollback();
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            if (this._autoRollback) this.Rollback();
            this.FreeChannel();
            this._owner.ResetContext();
        }

        private Lazy<IChannelProxy> GetChannel(string? connectionName, IMessage message)
        {
            if (this._lazyChannelProxy is null)
            {
                lock (this._syncObject)
                {
                    if (this._lazyChannelProxy is null)
                    {
                        this._lazyChannelProxy = new Lazy<IChannelProxy>(() =>
                        {
                            var c = this._channelPool.GetChannel(connectionName, message);
                            if (this._isTransaction) c.Channel?.TxSelect();
                            return c;
                        });
                    }
                }
            }
            return this._lazyChannelProxy;
        }

        private void FreeChannel()
        {
            this._lazyChannelProxy?.Value?.TryFree();
        }

        public override TTask CallAsync<TTask>(IMessage message, Func<IChannelProxy, IMessage, TTask> channelAction, string? connectionName)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (channelAction is null)
            {
                throw new ArgumentNullException(nameof(channelAction));
            }

            this.ThrowIfDisposed();

            if (this._isSubmited) throw new NotSupportedException("The operation is submited.");

            var channelProxy = this.GetChannel(connectionName, message).Value;

            if (!channelProxy.IsOpen)
            {
                this.FreeChannel();
                throw new InvalidOperationException("The channel is closed.");
            }

            try
            {
                return channelAction(channelProxy, message);
            }
            catch (Exception ex)
            {
                this.Manager.EmitError(this, ex);
                if (!channelProxy.IsOpen)
                {
                    this.FreeChannel();
                }
                throw;
            }
        }
    }

}
