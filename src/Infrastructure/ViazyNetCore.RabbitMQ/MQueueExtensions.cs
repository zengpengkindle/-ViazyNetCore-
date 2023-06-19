using System.Collections.Generic;
using System.Threading.Tasks;

using ViazyNetCore.RabbitMQ;
using ViazyNetCore.RabbitMQ.Declares;

using RabbitMQ.Client;
using System.Threading;

namespace System
{
    /// <summary>
    /// 表示消息队列的扩展库。
    /// </summary>
    public static class MQueueExtensions
    {

        internal static bool IsContinueOnError(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            //- 超时
            if (exception is TimeoutException) return false;
            if (exception is ObjectDisposedException) return false;
            //- 连接被关闭
            if (exception is global::RabbitMQ.Client.Exceptions.AlreadyClosedException) return false;
            if (exception is global::RabbitMQ.Client.Exceptions.BrokerUnreachableException) return false;
            if (exception is global::RabbitMQ.Client.Exceptions.RabbitMQClientException) return false;

            return true;
        }

        /// <summary>
        /// 设置交换机为 <see cref="ExchangeType.Topic"/> 类型。
        /// </summary>
        /// <param name="declare">交换机。</param>
        /// <returns>交换机。</returns>
        public static IExchangeDeclare UseTypeTopic(this IExchangeDeclare declare)
        {
            declare.Type = ExchangeType.Topic;
            return declare;
        }

        /// <summary>
        /// 设置交换机为 <see cref="ExchangeType.Headers"/> 类型。
        /// </summary>
        /// <param name="declare">交换机。</param>
        /// <returns>交换机。</returns>
        public static IExchangeDeclare UseTypeHeaders(this IExchangeDeclare declare)
        {
            declare.Type = ExchangeType.Headers;
            return declare;
        }

        /// <summary>
        /// 设置交换机为 <see cref="ExchangeType.Direct"/> 类型。
        /// </summary>
        /// <param name="declare">交换机。</param>
        /// <returns>交换机。</returns>
        public static IExchangeDeclare UseTypeDirect(this IExchangeDeclare declare)
        {
            declare.Type = ExchangeType.Direct;
            return declare;
        }

        /// <summary>
        /// 设置交换机为 <see cref="ExchangeType.Fanout"/> 类型。
        /// </summary>
        /// <param name="declare">交换机。</param>
        /// <returns>交换机。</returns>
        public static IExchangeDeclare UseTypeFanout(this IExchangeDeclare declare)
        {
            declare.Type = ExchangeType.Fanout;
            return declare;
        }

        /// <summary>
        /// 设置交换机为延迟消息类型。
        /// </summary>
        /// <param name="declare">交换机。</param>
        /// <param name="type">交换机转发类型。</param>
        /// <returns>交换机。</returns>
        public static IExchangeDeclare WithTypeDelayed(this IExchangeDeclare declare, string? type = null)
        {
            if (type is null) type = declare.Type ?? ExchangeType.Direct;
            declare.Type = "x-delayed-message";
            return declare.Arg("x-delayed-type", type);
        }

        /// <summary>
        /// 设置发布到队列的消息在被丢弃之前的存活时间(毫秒)。
        /// </summary>
        /// <param name="declare">队列。</param>
        /// <param name="value">存活时间。</param>
        /// <returns>队列。</returns>
        public static IQueueDeclare WithMessageTTL(this IQueueDeclare declare, long value)
        {
            return declare.Arg("x-message-ttl", value);
        }

        /// <summary>
        /// 设置队列在自动删除之前可以闲置多长时间(毫秒)。
        /// </summary>
        /// <param name="declare">队列。</param>
        /// <param name="value">闲置时间。</param>
        /// <returns>队列。</returns>
        public static IQueueDeclare WithAutoExprie(this IQueueDeclare declare, long value)
        {
            return declare.Arg("x-expires", value);
        }

        /// <summary>
        /// 当消息是死信时使用的可选替换路由键。如果未设置此选项，则将使用消息的原始路由密钥。
        /// </summary>
        /// <param name="declare">队列。</param>
        /// <param name="routeName">原始路由。</param>
        /// <returns>队列。</returns>
        public static IQueueDeclare WithDeadLetterRoutingKey(this IQueueDeclare declare, string routeName)
        {
            return declare.Arg("x-dead-letter-routing-key", routeName);
        }

        /// <summary>
        /// 可选的交换器名称，当消息被拒绝或过期时将重新发布到该交换器。
        /// </summary>
        /// <param name="declare">队列。</param>
        /// <param name="exchangeName">交换器。</param>
        /// <returns>队列。</returns>
        public static IQueueDeclare WithDeadLetterExchange(this IQueueDeclare declare, string exchangeName)
        {
            return declare.Arg("x-dead-letter-exchange", exchangeName);
        }

        /// <summary>
        /// 队列支持的最大优先级级别数;如果未设置，队列将不支持消息优先级。
        /// </summary>
        /// <param name="declare">队列。</param>
        /// <param name="exchangeName">优先级。</param>
        /// <returns>队列。</returns>
        public static IQueueDeclare WithMaxPriority(this IQueueDeclare declare, int priority = 10)
        {
            return declare.Arg("x-max-priority", priority);
        }

        /// <summary>
        /// 设置队列模型，可用值  <see langword="default"/>,<see langword="lazy"/>. 默认值<see langword="default"/>
        /// </summary>
        /// <param name="declare">队列。</param>
        /// <param name="queueMode"></param>
        /// <returns>队列。</returns>
        public static IQueueDeclare WithQueueMode(this IQueueDeclare declare, string queueMode = "default")
        {
            return declare.Arg("x-queue-mode", queueMode);
        }

        /// <summary>
        /// 设置队列类型，可用值 <see langword="classic"/>,<see langword="quorum"/>.
        /// </summary>
        /// <param name="declare"></param>
        /// <param name="queueType"></param>
        /// <returns></returns>
        public static IQueueDeclare WithQueueType(this IQueueDeclare declare, string queueType = "classic")
        {
            return declare.Arg("x-queue-type", queueType);
        }

        /// <summary>
        /// 添加一个参数。
        /// </summary>
        /// <typeparam name="TDeclare">交换机的数据类型。</typeparam>
        /// <param name="declare">交换机。</param>
        /// <param name="key">参数键名。</param>
        /// <param name="value">参数键值。</param>
        /// <returns>交换机。</returns>
        public static TDeclare Arg<TDeclare>(this TDeclare declare, string key, object value) where TDeclare : IDeclare
        {
            var args = declare.Arguments;
            if (args is null)
            {
                args = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                declare.Arguments = args;
            }
            args[key] = value;
            return declare;
        }

        /// <summary>
        /// 绑定指定的交换机到当前声明。
        /// </summary>
        /// <typeparam name="TDeclare">交换机的数据类型。</typeparam>
        /// <param name="declare">声明。</param>
        /// <param name="source">来源交换机。</param>
        /// <param name="routerKey">路由键名。</param>
        /// <param name="arguments">参数。</param>
        /// <returns>声明。</returns>
        public static TDeclare Bind<TDeclare>(this TDeclare declare, IExchangeDeclare source, string routerKey, Dictionary<string, object>? arguments = null) where TDeclare : IDeclare
        {
            declare.Sources.Add(new ExchangeSource(routerKey, source, arguments));
            return declare;
        }

        /// <summary>
        /// 发布消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="bus">消息总线。</param>
        /// <param name="body">实体。</param>
        /// <param name="routerKey">路由键名。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        public static Task PublishAsync<TBody>(this IMessageBusCore bus, TBody body, string? routerKey, CancellationToken cancellationToken = default)
        {
            return bus.PublishAsync(body, new PublishOptions { RouterKey = routerKey }, cancellationToken);
        }

        /// <summary>
        /// 发布延迟消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="bus">消息总线。</param>
        /// <param name="body">实体。</param>
        /// <param name="delaySeconds">延迟时间（秒）。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        public static Task PublishAsync<TBody>(this IMessageBusCore bus, TBody body, double delaySeconds, CancellationToken cancellationToken = default)
        {
            var delayTimeSpan = delaySeconds > 0 ? TimeSpan.FromSeconds(delaySeconds) : default;
            return bus.PublishAsync(body, delayTimeSpan, cancellationToken);
        }

        /// <summary>
        /// 发布延迟消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="bus">消息总线。</param>
        /// <param name="body">实体。</param>
        /// <param name="delayDateTime">预计延迟时间。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        public static Task PublishAsync<TBody>(this IMessageBusCore bus, TBody body, DateTime delayDateTime, CancellationToken cancellationToken = default)
        {
            var delayTimeSpan = default(TimeSpan);
            if (delayDateTime.Ticks > 0) delayTimeSpan = delayDateTime - DateTime.Now;
            return bus.PublishAsync(body, delayTimeSpan, cancellationToken);
        }

        /// <summary>
        /// 发布延迟消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="bus">消息总线。</param>
        /// <param name="body">实体。</param>
        /// <param name="delayTimeSpan">预计延迟时间。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        public static Task PublishAsync<TBody>(this IMessageBusCore bus, TBody body, TimeSpan delayTimeSpan, CancellationToken cancellationToken = default)
        {
            return bus.PublishAsync(body, new PublishOptions { DelayTimeSpan = delayTimeSpan }, cancellationToken);
        }

        /// <summary>
        /// 发布延迟消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="bus">消息总线。</param>
        /// <param name="body">实体。</param>
        /// <param name="routerKey">路由键名。</param>
        /// <param name="delaySeconds">延迟时间（秒）。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        public static Task PublishAsync<TBody>(this IMessageBusCore bus, TBody body, string? routerKey, double delaySeconds, CancellationToken cancellationToken = default)
        {
            var delayTimeSpan = delaySeconds > 0 ? TimeSpan.FromSeconds(delaySeconds) : default;
            return bus.PublishAsync(body, routerKey, delayTimeSpan, cancellationToken);
        }

        /// <summary>
        /// 发布延迟消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="bus">消息总线。</param>
        /// <param name="body">实体。</param>
        /// <param name="routerKey">路由键名。</param>
        /// <param name="delayDateTime">预计延迟时间。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        public static Task PublishAsync<TBody>(this IMessageBusCore bus, TBody body, string? routerKey, DateTime delayDateTime, CancellationToken cancellationToken = default)
        {
            var delayTimeSpan = default(TimeSpan);
            if (delayDateTime.Ticks > 0) delayTimeSpan = delayDateTime - DateTime.Now;
            return bus.PublishAsync(body, routerKey, delayTimeSpan, cancellationToken);
        }

        /// <summary>
        /// 发布延迟消息。
        /// </summary>
        /// <typeparam name="TBody">实体的数据类型。</typeparam>
        /// <param name="bus">消息总线。</param>
        /// <param name="body">实体。</param>
        /// <param name="routerKey">路由键名。</param>
        /// <param name="delayTimeSpan">预计延迟时间。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>异步任务。</returns>
        public static Task PublishAsync<TBody>(this IMessageBusCore bus, TBody body, string? routerKey, TimeSpan delayTimeSpan, CancellationToken cancellationToken = default)
        {
            return bus.PublishAsync(body, new PublishOptions { RouterKey = routerKey, DelayTimeSpan = delayTimeSpan }, cancellationToken);
        }
    }
}
