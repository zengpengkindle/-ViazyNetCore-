using System.Text;


namespace System.MQueue
{
    /// <summary>
    /// 定义一个消息序列化工具。
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// 反序列化。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <param name="bytes">字节数组。</param>
        /// <returns>反序列化后的实例。</returns>
        object? Deserialize(Type type, byte[] bytes);
        /// <summary>
        /// 序列化实例。
        /// </summary>
        /// <param name="body">实体。</param>
        /// <returns>字节数组。</returns>
        byte[] Serialize(object? body);
    }

    class JsonMessageSerializer : IMessageSerializer
    {
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public byte[] Serialize(object? body)
        {
            var json = JSON.Stringify(body);
            return this.Encoding.GetBytes(json);
        }

        public object? Deserialize(Type type, byte[] bytes)
        {
            if(type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var json = this.Encoding.GetString(bytes);
            return JSON.Parse(json, type);
        }
    }
}