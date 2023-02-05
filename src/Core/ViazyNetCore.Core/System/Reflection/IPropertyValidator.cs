namespace System.Reflection
{
    /// <summary>
    /// 定义一个属性的检验器。
    /// </summary>
    public interface IPropertyValidator
    {
        /// <summary>
        /// 获取或设置一个值，指示属性检验的顺序。排序越小排在越前面。
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// 检验指定属性的值。
        /// </summary>
        /// <param name="propertyMapper">属性的映射器。</param>
        /// <param name="instance">一个实例，null 值表示检验静态属性。</param>
        /// <param name="value">属性的值。</param>
        void Validate(PropertyMapper propertyMapper, object instance, object value);
    }
}