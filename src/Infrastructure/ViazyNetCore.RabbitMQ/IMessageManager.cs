using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.MQueue
{
    /// <summary>
    /// 定义一个消息管理器。
    /// </summary>
    public interface IMessageManager
    {
        /// <summary>
        /// 配置变化时发生。
        /// </summary>
        event EventHandler? Changed;
        /// <summary>
        /// 错误时发生。
        /// </summary>
        event ExceptionEventHandler? Error;

        /// <summary>
        /// 获取配置。
        /// </summary>
        MessageOptions Options { get; }
        /// <summary>
        /// 获取默认的连接代理工厂。
        /// </summary>
        IConnectionProxyFactory? Default { get; }
        /// <summary>
        /// 获取指定名称的连接工厂。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <returns>连接代理工厂。</returns>
        IConnectionProxyFactory? Get(string? name);
        /// <summary>
        /// 获取连接代理工厂列表。
        /// </summary>
        IEnumerable<IConnectionProxyFactory> Factories { get; }
        /// <summary>
        /// 触发异常。
        /// </summary>
        /// <param name="sender">事件来源。</param>
        /// <param name="exception">异常信息。</param>
        void EmitError(object sender, Exception exception);
    }

    abstract class MessageManagerBase : IMessageManager
    {
        private const string DEFAULT_NAME = nameof(Default);

        protected abstract ConcurrentDictionary<string, DefaultConnectionProxyFactory>? Factories { get; }

        public abstract MessageOptions Options { get; }

        public IConnectionProxyFactory? Default => this.Get(null);

        IEnumerable<IConnectionProxyFactory> IMessageManager.Factories => this.Factories is null
            ? Array.Empty<IConnectionProxyFactory>()
            : this.Factories.Values;

        public MessageManagerBase() { }

        public event EventHandler? Changed;
        protected virtual void EmitChanged()
        {
            this.Changed?.Invoke(this, EventArgs.Empty);
        }

        public event ExceptionEventHandler? Error;

        public void EmitError(object sender, Exception exception)
        {
            this.Error?.Invoke(sender, new ExceptionEventArgs(exception));
        }

        protected async Task FreeChannelsAsync(DefaultConnectionProxyFactory[]? connectionProxyFactories)
        {
            if(connectionProxyFactories is null) return;

            while(true)
            {
                await Task.Delay(TimeSpan.FromSeconds(this.Options.ChannelInactiveSeconds));
                try
                {
                    //- 开启预热模式时，至少会保留一个通道
                    if(this.Options.PreheatCount > 0 && connectionProxyFactories.Sum(p => p.Connections.Sum(c => c.ChannelCount)) == 1)
                    {
                        continue;
                    }
                    //if(connectionProxyFactories.SelectMany())
                    foreach(var factory in connectionProxyFactories)
                    {
                        foreach(var conn in factory.Connections.ToArray())
                        {
                            conn.FreeChannels(this.Options.ChannelInactiveSeconds);
                        }
                    }
                }
                catch(Exception)
                {

                }
            }
        }

        public IConnectionProxyFactory? Get(string? name) => this.Factories is not null && this.Factories.TryGetValue(name ?? DEFAULT_NAME, out var value) ? value : null;
    }

}
