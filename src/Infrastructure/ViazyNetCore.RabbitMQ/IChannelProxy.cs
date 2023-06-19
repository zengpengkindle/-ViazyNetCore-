using System.Threading;

using RabbitMQ.Client;

namespace ViazyNetCore.RabbitMQ
{
    /// <summary>
    /// 定义一个通道代理。
    /// </summary>
    public interface IChannelProxy
    {
        /// <summary>
        /// 获取唯一编号。
        /// </summary>
        string Id { get; }
        /// <summary>
        /// 获取连接代理。
        /// </summary>
        IConnectionProxy Connection { get; }
        /// <summary>
        /// 获取通道。
        /// </summary>
        IModel? Channel { get; }
        /// <summary>
        /// 获取最后活动时间。
        /// </summary>
        DateTime LastActiveTime { get; }
        /// <summary>
        /// 获取一个值，表示通道是否处于忙碌状态。
        /// </summary>
        bool IsBusy { get; }
        /// <summary>
        /// 获取一个值，表示通道是否已连接。
        /// </summary>
        bool IsOpen { get; }
        /// <summary>
        /// 获取忙碌原因。
        /// </summary>
        string? Reason { get; }
        /// <summary>
        /// 尝试将通道设置为忙碌。
        /// </summary>
        /// <param name="reason">忙碌原因。</param>
        /// <returns>设置成功返回 <see langword="true"/> 值，否则返回 <see langword="false"/> 值。</returns>
        bool TryBusy(string reason);
        /// <summary>
        /// 尝试将通道设置为空闲。
        /// </summary>
        /// <returns>设置成功返回 <see langword="true"/> 值，否则返回 <see langword="false"/> 值。</returns>
        bool TryFree();
    }


    class DefaultChannelProxy : IChannelProxy
    {
        private const long BUSY_TRUE = 1L;
        private const long BUSY_FALSE = 0L;

        private readonly WeakReference<IModel> _weakChannel;
        private readonly IConnectionProxy _connectionProxy;
        private long _isBusy;

        public DefaultChannelProxy(IConnectionProxy connectionProxy, IModel channel)
        {
            this._weakChannel = new WeakReference<IModel>(channel);
            this._connectionProxy = connectionProxy;
            this.LastActiveTime = DateTime.Now;
            this.Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; }

        public IConnectionProxy Connection => this._connectionProxy;

        public bool IsOpen => this.Channel?.IsOpen == true;

        public bool IsBusy => Interlocked.Read(ref this._isBusy) == BUSY_TRUE;

        public IModel? Channel => this._weakChannel.TryGetTarget(out var target) ? target : null;

        public DateTime LastActiveTime { get; private set; }
        public string? Reason { get; private set; }

        private DefaultChannelProxy Active(string? reason)
        {
            this.Reason = reason;
            this.LastActiveTime = DateTime.Now;
            return this;
        }

        public bool TryBusy(string reason)
        {
            if(reason is null)
            {
                throw new ArgumentNullException(nameof(reason));
            }

            if(Interlocked.CompareExchange(ref this._isBusy, BUSY_TRUE, BUSY_FALSE) == BUSY_FALSE)
            {
                this.Active(reason);
                this._connectionProxy.IncrementChannel();
                return true;
            }
            return false;
        }

        public bool TryFree()
        {
            if(Interlocked.CompareExchange(ref this._isBusy, BUSY_FALSE, BUSY_TRUE) == BUSY_TRUE)
            {
                this.Active(null);
                this._connectionProxy.DecrementChannel();
                return true;
            }
            return false;
        }
    }
}
