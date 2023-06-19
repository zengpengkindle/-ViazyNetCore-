using System.Collections.Generic;

namespace ViazyNetCore.RabbitMQ.Declares
{
    /// <summary>
    /// 定义一个声明。
    /// </summary>
    public interface IDeclare
    {
        /// <summary>
        /// 获取声明的名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取声明的参数集合。
        /// </summary>
        Dictionary<string, object>? Arguments { get; set; }

        /// <summary>
        /// 获取声明的来源交换机集合。
        /// </summary>
        IList<IExchangeSource> Sources { get; }

        /// <summary>
        /// 在当前通道上创建一个声明。
        /// </summary>
        /// <param name="channelProxy">通道代理。</param>
        void Build(IChannelProxy channelProxy);
    }


    /// <summary>
    /// 表示一个声明的基类。
    /// </summary>
    public abstract class DeclareBase : IDeclare
    {
        private IList<IExchangeSource>? _sources;

        /// <inheritdoc />
        public virtual string Name { get; }

        /// <inheritdoc />
        public virtual Dictionary<string, object>? Arguments { get; set; }

        /// <inheritdoc />
        public virtual IList<IExchangeSource> Sources => this._sources ??= new List<IExchangeSource>();

        /// <summary>
        /// 初始化一个 <see cref="DeclareBase"/> 类的新实例。
        /// </summary>
        /// <param name="name">声明的名称。</param>
        protected DeclareBase(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <inheritdoc />
        public void Build(IChannelProxy channelProxy)
        {
            if(channelProxy is null)
            {
                throw new ArgumentNullException(nameof(channelProxy));
            }

            this.OnBuild(channelProxy);

            var sources = this._sources;
            if(sources is not null)
            {
                foreach(var source in sources)
                {
                    source.Exchange.Build(channelProxy);
                    this.OnBind(channelProxy, source);
                }
            }
        }

        /// <summary>
        /// 绑定时发生。
        /// </summary>
        /// <param name="channelProxy">通道代理。</param>
        /// <param name="source">绑定的源。</param>
        protected abstract void OnBind(IChannelProxy channelProxy, IExchangeSource source);

        /// <summary>
        /// 创建声明时发生。
        /// </summary>
        /// <param name="channelProxy">通道代理。</param>
        protected abstract void OnBuild(IChannelProxy channelProxy);
    }
}
