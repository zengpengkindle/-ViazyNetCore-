using System.Collections.Generic;

namespace System.MQueue.Declares
{
    /// <summary>
    /// 定义一个交换机来源。
    /// </summary>
    public interface IExchangeSource
    {
        /// <summary>
        /// 获取路由键名。
        /// </summary>
        public string RouterKey { get; }

        /// <summary>
        /// 获取来源的交换机声明。
        /// </summary>
        public IExchangeDeclare Exchange { get; }

        /// <summary>
        /// 获取绑定的参数集合。
        /// </summary>
        public IDictionary<string, object>? Arguments { get; }
    }

    /// <summary>
    /// 表示一个交换机来源。
    /// </summary>
    public sealed class ExchangeSource : IExchangeSource
    {
        /// <summary>
        /// 初始化一个 <see cref="ExchangeSource"/> 类的新实例。
        /// </summary>
        /// <param name="routerKey">路由键名。</param>
        /// <param name="source">来源的交换机声明。</param>
        /// <param name="arguments">绑定的参数集合。</param>
        public ExchangeSource(string? routerKey, IExchangeDeclare source, IDictionary<string, object>? arguments = null)
        {
            this.RouterKey = routerKey ?? string.Empty;
            this.Exchange = source ?? throw new ArgumentNullException(nameof(source));
            this.Arguments = arguments;
        }

        /// <inheritdoc />
        public string RouterKey { get; }

        /// <inheritdoc />
        public IExchangeDeclare Exchange { get; }

        /// <inheritdoc />
        public IDictionary<string, object>? Arguments { get; }
    }
}
