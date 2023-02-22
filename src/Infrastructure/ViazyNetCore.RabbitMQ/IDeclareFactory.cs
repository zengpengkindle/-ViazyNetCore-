using System.Collections.Concurrent;

using System.MQueue.Declares;

namespace System.MQueue
{
    /// <summary>
    /// 定义一个声明工厂。
    /// </summary>
    public interface IDeclareFactory
    {
        /// <summary>
        /// 获取所有的交换机。
        /// </summary>
        ConcurrentDictionary<string, IExchangeDeclare> Exchanges { get; }
        /// <summary>
        /// 获取所有的队列。
        /// </summary>
        ConcurrentDictionary<string, IQueueDeclare> Queues { get; }

        /// <summary>
        /// 创建或获取指定名称的队列。
        /// </summary>
        /// <param name="queueName">队列名称。</param>
        /// <returns>队列的声明。</returns>
        IQueueDeclare Queue(string queueName);
        /// <summary>
        /// 创建或获取指定名称的交换机。
        /// </summary>
        /// <param name="exchangeName">交换机名称。</param>
        /// <returns>交换机的声明。</returns>
        IExchangeDeclare Exchange(string exchangeName);
    }

    class DefaultDeclareFactory : IDeclareFactory
    {
        public ConcurrentDictionary<string, IExchangeDeclare> Exchanges { get; }
        public ConcurrentDictionary<string, IQueueDeclare> Queues { get; }

        public DefaultDeclareFactory()
        {
            this.Exchanges = new ConcurrentDictionary<string, IExchangeDeclare>(StringComparer.OrdinalIgnoreCase);
            this.Queues = new ConcurrentDictionary<string, IQueueDeclare>(StringComparer.OrdinalIgnoreCase);
        }

        private IQueueDeclare CreateQueue(string name) => new QueueDeclare(name)
        {
            Durable = true,
        };

        public IQueueDeclare Queue(string queueName)
        {
            if(queueName is null)
            {
                throw new ArgumentNullException(nameof(queueName));
            }

            return this.Queues.GetOrAdd(queueName, this.CreateQueue);
        }

        private IExchangeDeclare CreateExchange(string name) => new ExchangeDeclare(name)
        {
            Durable = true,
        };

        public IExchangeDeclare Exchange(string exchangeName)
        {
            if(exchangeName is null)
            {
                throw new ArgumentNullException(nameof(exchangeName));
            }

            return this.Exchanges.GetOrAdd(exchangeName, this.CreateExchange);
        }
    }
}