
using RabbitMQ.Client;

namespace ViazyNetCore.RabbitMQ
{
    /// <summary>
    /// 表示一个发布的选项。
    /// </summary>
    public class PublishOptions
    {
        /// <summary>
        /// 获取或设置一个值，表示连接名称。
        /// </summary>
        /// <value>当没有设置值时，将取默认连接。</value>
        public string? ConnectionName { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示交换机名称。
        /// </summary>
        /// <value>当没有设置值时，将根据输入类型自动生成交换机。</value>
        public string? Exchange { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示路由键名。
        /// </summary>
        /// <value>当没有设置值时，将使用 <see cref="string.Empty"/> 作为的路由键名。</value>
        public string? RouterKey { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示是否强制消费。
        /// </summary>
        /// <value>设置为 <see langword="true"/> 值时，如果交换机根据自身类型和消息 <see cref="RouterKey"/> 无法找到一个符合条件的队列，那么会调用 <see cref="global::RabbitMQ.Client.IModel.BasicReturn"/> 方法将消息返回给生产者；当设置为 <see langword="false"/> 值时，出现上述情形会直接将消息扔掉。</value>
        public bool Mandatory { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示延迟时间。
        /// </summary>
        public TimeSpan DelayTimeSpan { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示属性配置。
        /// </summary>
        public Action<IBasicProperties>? PropertiesSetup { get; set; }
    }

    /// <summary>
    /// 表示一个订阅的选项。
    /// </summary>
    public class SubscribeOptions
    {
        /// <summary>
        /// 获取或设置一个值，表示连接名称。
        /// </summary>
        /// <remarks>详细文档请参考<see href="https://www.throwable.club/2018/11/28/rabbitmq-extension-consumer-prefetch/">消费者消息预读取</see>链接。</remarks>
        /// <value>当没有设置值时，将取默认连接。</value>
        public string? ConnectionName { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示预读取的消息数量上限。
        /// </summary>
        /// <value>默认为 0 值，表示无上限。</value>
        public ushort QosPrefetchCount { get; set; } = 0;

        /// <summary>
        /// 获取或设置一个值，表示是否自动应答。
        /// </summary>
        /// <value>默认为 <see langword="true"/> 值"/>，表示自动应答。</value>
        public bool AutoAck { get; set; } = true;

        /// <summary>
        /// 获取或设置一个值，表示是否强制消费。
        /// </summary>
        /// <value>设置为 <see langword="true"/> 值时，如果交换机根据自身类型和消息路由无法找到一个符合条件的队列，那么会调用 <see cref="global::RabbitMQ.Client.IModel.BasicReturn"/> 方法将消息返回给生产者；当设置为 <see langword="false"/> 值时，出现上述情形会直接将消息扔掉。</value>
        public bool Mandatory { get; set; }
    }
}
