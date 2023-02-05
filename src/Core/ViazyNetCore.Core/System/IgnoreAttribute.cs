namespace System
{
    /// <summary>
    /// 表示一个具有忽略性的标识。
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class IgnoreAttribute : ComponentModel.DataAnnotations.Schema.NotMappedAttribute
    {
        //Text.Json.Serialization.JsonIgnoreAttribute
        /// <summary>
        /// 初始化一个 <see cref="IgnoreAttribute"/> 类的新实例。
        /// </summary>
        public IgnoreAttribute() { }
    }
}