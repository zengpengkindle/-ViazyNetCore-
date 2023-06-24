using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System;
using RabbitMQ.Client;

namespace ViazyNetCore.RabbitMQ
{
    /// <summary>
    /// 定义一个连接的代理。
    /// </summary>
    public interface IConnectionProxy : IDisposable
    {
        /// <summary>
        /// 获取连接代理的编号。
        /// </summary>
        string Id { get; }
        /// <summary>
        /// 获取一个值，表示连接是否已连接。
        /// </summary>
        bool IsOpen { get; }
        /// <summary>
        /// 获取连接。
        /// </summary>
        IConnection? Connection { get; }
        /// <summary>
        /// 获取通道使用量。
        /// </summary>
        long UsageChannelCount { get; }
        /// <summary>
        /// 获取通道总量。
        /// </summary>
        long ChannelCount { get; }
        /// <summary>
        /// 获取通道忙碌总量。
        /// </summary>
        long BusyCount { get; }
        /// <summary>
        /// 获取通道空闲总量。
        /// </summary>
        long FreeCount { get; }

        /// <summary>
        /// 获取所有的通道代理。
        /// </summary>
        IEnumerable<IChannelProxy> Channels { get; }
        /// <summary>
        /// 获取连接代理工厂。
        /// </summary>
        IConnectionProxyFactory Factory { get; }

        /// <summary>
        /// 递增一个忙碌的通道。
        /// </summary>
        /// <returns>通道忙碌总量。</returns>
        long IncrementChannel();
        /// <summary>
        /// 递减一个忙碌的通道。
        /// </summary>
        /// <returns>通道忙碌总量。</returns>
        long DecrementChannel();

        /// <summary>
        /// 获取一个通道代理。
        /// </summary>
        /// <param name="awalysNewChannel">必然返回新的通道代理。</param>
        /// <returns>通道代理。</returns>
        IChannelProxy? GetChannel(bool awalysNewChannel);
        /// <summary>
        /// 关闭所有满足条件的空闲通道。
        /// </summary>
        /// <param name="inactiveSeconds">空闲秒数。</param>
        void FreeChannels(int inactiveSeconds);
        /// <summary>
        /// 判断指定的消息是否已在当前连接声明。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <returns>已声明返回 <see langword="true"/> 值，否则返回 <see langword="false"/> 值。</returns>
        bool IsDeclareBuild(IMessage message);
    }

    class DefaultConnectionProxy : ObjectDisposableBase, IConnectionProxy
    {
        private readonly WeakReference<IConnection> _weakConnection;
        private readonly ConcurrentDictionary<string, IChannelProxy> _channels;
        private readonly DefaultConnectionProxyFactory _connectionProxyFactory;
        private readonly ConcurrentDictionary<string, bool> _declareMessages;
        private long _channelUsageTimes;

        public DefaultConnectionProxy(string clientProviderName, IConnection connection, DefaultConnectionProxyFactory connectionProxyFactory)
        {
            this.Id = Guid.NewGuid().ToString("N");
            this._weakConnection = new WeakReference<IConnection>(connection);
            this._channels = new ConcurrentDictionary<string, IChannelProxy>();
            this.ClientProviderName = clientProviderName;
            this._connectionProxyFactory = connectionProxyFactory;
            this._declareMessages = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        }

        private bool TryGetConnection([NotNullWhen(true)] out IConnection? connection)
        {
            var weakConn = this._weakConnection;
            if(weakConn is not null && weakConn.TryGetTarget(out connection) && connection is not null) return true;
            connection = null;
            return false;
        }

        public IConnectionProxyFactory Factory => this._connectionProxyFactory;
        public string Id { get; }

        public IConnection? Connection => this.TryGetConnection(out var target) ? target : null;

        public bool IsOpen => this.Connection?.IsOpen == true;

        public long UsageChannelCount => Interlocked.Read(ref this._channelUsageTimes);

        public long BusyCount => this._channels.Values.Count(c => c.IsBusy);

        public long FreeCount => this._channels.Values.Count(c => !c.IsBusy);

        public long ChannelCount => this._channels.Count;

        public IEnumerable<IChannelProxy> Channels => this._channels.Values;

        public string ClientProviderName { get; }

        public long IncrementChannel()
        {
            return Interlocked.Increment(ref this._channelUsageTimes);
        }

        public long DecrementChannel()
        {
            return Interlocked.Decrement(ref this._channelUsageTimes);
        }

        private IChannelProxy? CreateChennel(bool autoNext)
        {
            lock(this)
            {
                var conn = this.Connection;
                if(conn?.IsOpen == true && this._channels.Count <= this._connectionProxyFactory.Manager.Options.MaxChannelPerConnection)
                {
                    DefaultChannelProxy? channel = null;
                    for(int i = 0; i < this._connectionProxyFactory.Manager.Options.ChannelCreateRetryCount; i++)
                    {
                        try
                        {
                            channel = new DefaultChannelProxy(this, conn.CreateModel());
                            break;
                        }
                        catch(Exception ex)
                        {
                            this._connectionProxyFactory.Manager.EmitError(this, ex);

                            if(!MQueueExtensions.IsContinueOnError(ex))
                            {
                                this.Connection?.Dispose();
                                break;
                            }
                            if(conn.IsOpen)
                            {
                                GA.Wait(this._connectionProxyFactory.Manager.Options.ChannelCreateRetrySeconds);
                            }
                        }
                    }
                    if(channel is null) return null;

                    if(!channel.IsOpen) throw new InvalidOperationException($"The message connection is open, but the is closed when create a new channel.");
                    if(channel.TryBusy("Create-" + (autoNext ? "Automatic" : "Manual")))
                    {
                        this._channels.TryAdd(channel.Id, channel);
                        if(!autoNext && (this.ChannelCount - this.UsageChannelCount) < 2)
                        {
                            Task.Factory.StartNew(() => this.CreateChennel(true));
                        }
                        if(autoNext) channel.TryFree();
                        return channel;
                    }

                }
                return null;
            }
        }

        public IChannelProxy? GetChannel(bool awalysNewChannel)
        {
            if(!this.IsOpen) return null;

            IChannelProxy? channel = null;
            if(!awalysNewChannel && !this._channels.IsEmpty) channel = this._channels.Values.FirstOrDefault(c => c.TryBusy("Recapture"));
            if(channel is null) channel = this.CreateChennel(false);

            if(channel is not null && !channel.IsOpen)
            {
                this.RemoveChannel(channel);
            }

            return channel;
        }

        private void RemoveChannel(IChannelProxy channel)
        {
            this._channels.TryRemove(channel.Id, out _);
            channel.TryFree();
        }

        public void FreeChannels(int inactiveSeconds)
        {
            if(inactiveSeconds < 1)
                throw new ArgumentOutOfRangeException(nameof(inactiveSeconds));

            if(!this._channels.IsEmpty)
            {
                var now = GA.Now;
                var preheatCount = this.Factory.Manager.Options.PreheatCount;
                foreach(var channel in this._channels.Values.Where(c => !c.IsOpen || (!c.IsBusy && (now - c.LastActiveTime).TotalSeconds >= inactiveSeconds) && c.TryBusy("OnFree")).ToArray())
                {
                    //- 开启预热模式时，至少会保留一个通道
                    if(preheatCount > 0 && this._connectionProxyFactory.Connections.Sum(p => p.ChannelCount) == 1)
                    {
                        channel.TryFree();
                        continue;
                    }

                    this.RemoveChannel(channel);

                    try
                    {
                        var raw = channel.Channel;
                        if(raw is not null && raw.IsOpen)
                        {
                            raw.Close();
                            raw.Dispose();
                        }
                    }
                    catch(Exception) { }
                }
            }
        }

        public bool IsDeclareBuild(IMessage message)
        {
            if(message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }
           
            return !this._declareMessages.TryAdd(message.Id, true);
        }

        protected override void DisposeManaged()
        {
            if(this.TryGetConnection(out var t))
            {
                try
                {
                    t.Close();
                }
                catch(Exception) { }

                try
                {
                    t?.Dispose();
                }
                catch(Exception) { }
            }
        }
    }
}
