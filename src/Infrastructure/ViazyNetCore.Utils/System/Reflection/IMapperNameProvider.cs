namespace System.Reflection
{
    /// <summary>
    /// 定义一个类型映射器的名称提供程序。
    /// </summary>
    public interface IMapperNameProvider
    {
        /// <summary>
        /// 获取指定来源、类型映射器和实体的映射器名称。
        /// </summary>
        /// <param name="source">来源。可以是一个 NULL 值。</param>
        /// <param name="mapper">类型映射器。</param>
        /// <param name="entity">类型映射器关联的对象实例，并将 <paramref name="entity"/> 转换成映射器的类型 。根据不同的来源，可能是一个 null 值。</param>
        /// <returns>映射器名称。</returns>
        string GetName(string source, TypeMapper mapper, object entity);
    }
}