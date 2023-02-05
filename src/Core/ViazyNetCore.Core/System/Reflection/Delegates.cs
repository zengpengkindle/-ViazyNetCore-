namespace System
{
    /// <summary>
    /// 表示一个动态方法调用的委托。
    /// </summary>
    /// <param name="instance">调用的实例。使用 null 值表示调用静态方法。</param>
    /// <param name="parameters">方法的参数列表。</param>
    /// <returns>方法执行的结果，如果方法是一个 void 签名则返回 null 值。</returns>
    public delegate object DynamicMethodInvoker(object instance, params object[] parameters);
    /// <summary>
    /// 表示一个动态成员的获取器委托。
    /// </summary>
    /// <param name="instance">调用的实例。使用 null 值表示获取静态成员。</param>
    /// <returns>成员的值。</returns>
    public delegate object DynamicMemberGetter(object instance);
    /// <summary>
    /// 表示一个动态成员的设置器委托。
    /// </summary>
    /// <param name="instance">调用的实例。使用 null 值表示设置静态成员。</param>
    /// <param name="value">设置的值。</param>
    public delegate void DynamicMemberSetter(object instance, object value);
    /// <summary>
    /// 表示动态创建实例的委托。
    /// </summary>
    /// <param name="parameters">构造函数的参数值集合。</param>
    /// <returns>动态创建的实例。</returns>
    public delegate object DynamicConstructorHandler(params object[] parameters);
}