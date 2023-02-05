namespace System
{
    /// <summary>
    /// 表示一个具备主键的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class KeyAttribute : Attribute
    {
        /// <summary>
        /// 初始化一个 <see cref="KeyAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="name">名称。</param>
        public KeyAttribute(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// 获取名称。
        /// </summary>
        public string Name { get; }
    }
}