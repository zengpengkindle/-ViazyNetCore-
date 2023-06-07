namespace ViazyNetCore
{
    /// <summary>
    /// 表示一个消息的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MessageAttribute : Attribute
    {
        /// <summary>
        /// 获取或设置一个值，表示消息编号。
        /// </summary>
        /// <value>当没有设置值时，将根据输入类型自动生成消息编号。</value>
        public string? Id { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示交换机名称。
        /// </summary>
        /// <value>当没有设置值时，将根据输入类型自动生成交换机。</value>
        public string? Exchange { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示队列名称。
        /// </summary>
        /// <value>当没有设置值时，将根据输入类型自动队列。</value>
        public string? Queue { get; set; }

        public string? RouterKey { get; set; }
    }
}
