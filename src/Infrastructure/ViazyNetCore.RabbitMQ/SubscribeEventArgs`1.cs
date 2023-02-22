
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace System.MQueue
{
    /// <summary>
    /// 表示消息订阅的事件参数。
    /// </summary>
    /// <typeparam name="TBody">实体的数据类型。</typeparam>
    public class SubscribeEventArgs<TBody> : EventArgs
    {
        /// <summary>
        /// 获取或设置一个值，表示真实的订阅事件参数。
        /// </summary>
        public BasicDeliverEventArgs? Raw { get; set; }
        /// <summary>
        /// 获取基础属性。
        /// </summary>
        public IBasicProperties? BasicProperties => this.Raw?.BasicProperties;

        /// <summary>
        /// 获取或设置一个值，表示消息实体。
        /// <para>该值如果为默认值，则表示数据转换失败。</para>
        /// </summary>
        public TBody? Body { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示是否应答成功。
        /// </summary>
        public bool Ack { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示是否为重新入队标识。
        /// <para>表示已被其他消息队列处理，但是处理失败，并且重新入队。</para>
        /// </summary>
        public bool Redelivered { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示重新入队的次数。
        /// <para>只有 <see cref="Redelivered"/> 值为 <see langword="true"/> 时，该属性才会有数据。否则永远为 0 值。</para>
        /// </summary>
        public int RedeliveredCount { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示批量应答或批量取消应答。
        /// <para>设置为 <see langword="true"/> 值时，将会时下次重新入队的头部信息为原始头部信息。</para>
        /// </summary>
        public bool Multiple { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示取消应答是否重新入队。
        /// <para>默认为 <see langword="true"/> 值，当发生异常时消息将会重新返回队列。如不希望返回队列，应手动捕获错误，并将该属性设置为 <see langword="false"/> 值。</para>
        /// <para>当手动应答模式，并且 <see cref="Ack"/> 值为 <see langword="true"/> 时，该属性无效。</para>
        /// </summary>
        public bool Requeue { get; set; } = true;

    }

}
