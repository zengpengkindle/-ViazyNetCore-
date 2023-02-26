using System.Collections.Generic;

namespace System.Repos
{
    /// <summary>
    /// 定义一个仓储提供程序。
    /// </summary>
    public interface IRepositoryProvider
    {
        /// <summary>
        /// 获取仓储。
        /// </summary>
        /// <typeparam name="TEntity">仓储的数据类型。</typeparam>
        /// <param name="nameBuilder">集合的名称生成器。</param>
        /// <returns>仓储的实例。</returns>
        IRepository<TEntity, TEntity> Create<TEntity>(Func<TypeMapper, string>? nameBuilder = null);
    }

    /// <summary>
    /// 定义一个仓储管理器。
    /// </summary>
    public interface IRepositoryManager
    {
        /// <summary>
        /// 初始化仓储。
        /// </summary>
        /// <param name="connections">连接池管理。</param>
        void Initialize(IEnumerable<KeyValuePair<string, string>> connections);
        /// <summary>
        /// 获取指定数据库的仓储提供程序。
        /// </summary>
        /// <param name="database">数据库名称。</param>
        /// <returns>仓储提供程序。</returns>
        IRepositoryProvider GetProvider(string? database);
    }
}
