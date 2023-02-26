using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Repos
{
    /// <summary>
    /// 定义一个仓储。
    /// </summary>
    /// <typeparam name="TEntity">实体的数据类型。</typeparam>
    /// <typeparam name="TElement">元素的数据类型。</typeparam>
    public interface IRepository<TEntity, TElement> : IRepositoryQuery<TEntity, TElement>
    {
        /// <summary>
        /// 添加新的实体。
        /// </summary>
        /// <param name="entity">实体。</param>
        void Add(TEntity entity);

        /// <summary>
        /// 添加新的实体。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <returns>异步任务。</returns>
        Task AddAsync(TEntity entity);
        /// <summary>
        /// 批量添加新的实体。
        /// </summary>
        /// <param name="entities">实体集合。</param>
        void AddRange(IEnumerable<TEntity> entities);
        /// <summary>
        /// 批量添加新的实例。
        /// </summary>
        /// <param name="entities">实体集合。</param>
        /// <returns>异步任务。</returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}
