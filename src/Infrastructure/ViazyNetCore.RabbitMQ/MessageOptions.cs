namespace ViazyNetCore.RabbitMQ
{
    /// <summary>
    /// 表示一个消息的配置。
    /// </summary>
    public class MessageOptions
    {

        /// <summary>
        /// 获取或设置一个值，表示预热通道数。
        /// </summary>
        /// <value>默认为 10 值"，表示开启 1 个连接的 10 个通道。设置为 0 值时，表示关闭预热。</value>
        public uint PreheatCount { get; set; } = 10;

        /// <summary>
        /// 获取或设置一个值，表示每个连接最多可以创建的通道数量。
        /// </summary>
        /// <value>默认为 211 个通道。</value>
        public int MaxChannelPerConnection { get; set; } = 211;
        /// <summary>
        /// 获取或设置一个值，表示通道空闲失效时间。
        /// </summary>
        /// <value>默认为 180 秒。</value>
        public int ChannelInactiveSeconds { get; set; } = 180;

        /// <summary>
        /// 获取或设置一个值，表示发布重试的次数。
        /// </summary>
        /// <value>默认为 3 次。</value>
        public int PublishRetryCount { get; set; } = 3;
        /// <summary>
        /// 获取或设置一个值，表示发布每次重试的间隔。
        /// </summary>
        /// <value>默认为 3 秒。</value>
        public int PublishRetrySeconds { get; set; } = 3;

        /// <summary>
        /// 获取或设置一个值，表示通道创建重试的次数。
        /// </summary>
        /// <value>默认为 3 次。</value>
        public int ChannelCreateRetryCount { get; set; } = 3;
        /// <summary>
        /// 获取或设置一个值，表示通道创建每次重试的间隔。
        /// </summary>
        /// <value>默认为 3 秒。</value>
        public int ChannelCreateRetrySeconds { get; set; } = 3;

        /// <summary>
        /// 获取或设置一个值，表示连接创建重试的次数。
        /// </summary>
        /// <value>默认为 3 次。</value>
        public int ConnectionCreateRetryCount { get; set; } = 3;
        /// <summary>
        /// 获取或设置一个值，表示连接创建每次重试的间隔。
        /// </summary>
        /// <value>默认为 3 秒。</value>
        public int ConnectionCreateRetrySeconds { get; set; } = 3;
    }
}
