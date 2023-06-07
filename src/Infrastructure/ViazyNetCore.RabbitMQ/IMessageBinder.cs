using System.MQueue;
using System.MQueue.Declares;
using RabbitMQ.Client;

namespace System
{
    /// <summary>
    /// 定义一个消息绑定器。
    /// </summary>
    public interface IMessageBinder
    {
        /// <summary>
        /// 获取绑定排序。
        /// </summary>
        /// <value>值越小越先执行。</value>
        int Order { get; }
        /// <summary>
        /// 绑定消息。
        /// </summary>
        /// <param name="declareFactory">声明工厂。</param>
        /// <param name="message">消息。</param>
        /// <param name="queue">队列。</param>
        /// <returns>队列。</returns>
        IQueueDeclare Bind(IDeclareFactory declareFactory, IMessage message, IQueueDeclare queue);
    }

    /// <summary>
    /// 表示一个允许延迟消费消息的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AllowDelayedAttribute : Attribute, IMessageBinder
    {
        int IMessageBinder.Order => 0;

        IQueueDeclare IMessageBinder.Bind(IDeclareFactory declareFactory, IMessage message, IQueueDeclare queue)
        {
            queue.Sources.Add(new ExchangeSource(null, declareFactory.Exchange(message.Exchange).WithTypeDelayed(), null));
            return queue;
        }
    }

    /// <summary>
    /// 表示一个允许延迟消费消息的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TopicMessageAttribute : Attribute, IMessageBinder
    {
        int IMessageBinder.Order => 0;

        /// <summary>
        /// 获取或设置路由规则。
        /// </summary>
        public string? RouterKey { get; set; }
        /// <summary>
        /// 获取或设置交换机名称。
        /// </summary>
        public string? Exchange { get; set; }

        IQueueDeclare IMessageBinder.Bind(IDeclareFactory declareFactory, IMessage message, IQueueDeclare queue)
        {
            var declare = declareFactory.Exchange(this.Exchange ?? message.Exchange)
                .UseTypeTopic();
            queue.Sources.Add(new ExchangeSource(this.RouterKey ?? message.Exchange, declare, null));
            return queue;
        }
    }
}
