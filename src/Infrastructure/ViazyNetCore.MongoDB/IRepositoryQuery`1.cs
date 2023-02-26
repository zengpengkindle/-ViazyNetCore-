using System.Linq;
using System.Threading.Tasks;

namespace System.Repos
{
    /// <summary>
    /// 定义一个仓储查询。
    /// </summary>
    /// <typeparam name="TElement">元素的数据类型。</typeparam>
    public interface IRepositoryQuery<TElement>
    {
        /// <summary>
        /// 获取可查询元素集合。
        /// </summary>
        IQueryable<TElement> Queryable { get; }
        /// <summary>
        /// 执行分页操作。
        /// </summary>
        /// <param name="pageNumber">起始页。</param>
        /// <param name="pageSize">分页大小。</param>
        /// <returns>分页结果。</returns>
        PageData<TElement> ToPage(int pageNumber = 1, int pageSize = 10);
        /// <summary>
        /// 异步执行分页操作。
        /// </summary>
        /// <param name="pageNumber">起始页。</param>
        /// <param name="pageSize">分页大小。</param>
        /// <returns>异步分页结果。</returns>
        Task<PageData<TElement>> ToPageAsync(int pageNumber = 1, int pageSize = 10);
        /// <summary>
        /// 创建仓储查询。
        /// </summary>
        /// <typeparam name="TOutput">输出的元素类型。</typeparam>
        /// <param name="queryable">可查询集合。</param>
        /// <returns>可查询仓储。</returns>
        IRepositoryQuery<TOutput> CreateQuery<TOutput>(IQueryable<TOutput> queryable);
    }

    /// <summary>
    /// 定义一个可查询仓储。
    /// </summary>
    /// <typeparam name="TEntity">实体的数据类型。</typeparam>
    /// <typeparam name="TElement">查询元素的数据类型。</typeparam>
    public interface IRepositoryQuery<TEntity, TElement> : IRepositoryQuery<TElement>
    {
        /// <summary>
        /// 创建新的可查询仓储。
        /// </summary>
        /// <typeparam name="TOutput">输出的元素类型。</typeparam>
        /// <param name="queryable">可查询仓储。</param>
        /// <returns>可查询仓储。</returns>
        new IRepositoryQuery<TEntity, TOutput> CreateQuery<TOutput>(IQueryable<TOutput> queryable);
        /// <summary>
        /// 随机获取元素。
        /// </summary>
        /// <param name="count">随机的数量。</param>
        /// <returns>可查询仓储。</returns>
        IRepositoryQuery<TEntity, TElement> Random(long count);

        /// <summary>
        /// 基于当前查询的修改操作。
        /// </summary>
        /// <param name="modifyExpression">修改的表达式。</param>
        /// <param name="upsert">不存在时是否执行插入操作。</param>
        /// <returns>受影响的元素数。</returns>
        long Modify(Action<IModifyExpression<TEntity>> modifyExpression, bool upsert = false);
        /// <summary>
        /// 基于当前查询的修改操作。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <param name="upsert">不存在时是否执行插入操作。</param>
        /// <returns>受影响的元素数。</returns>
        long Modify(object entity, bool upsert = false);
        /// <summary>
        /// 基于当前查询的批量修改。
        /// </summary>
        /// <param name="modifyExpression">修改的表达式。</param>
        /// <param name="upsert">不存在时是否执行插入操作。</param>
        /// <returns>受影响的元素数。</returns>
        long ModifyRange(Action<IModifyExpression<TEntity>> modifyExpression, bool upsert = false);
        /// <summary>
        /// 基于当前查询的批量修改。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <param name="upsert">不存在时是否执行插入操作。</param>
        /// <returns>受影响的元素数。</returns>
        long ModifyRange(object entity, bool upsert = false);

        /// <summary>
        /// 基于当前查询的删除操作。
        /// </summary>
        /// <returns>受影响的元素数。</returns>
        long Remove();
        /// <summary>
        /// 基于当前查询的批量删除操作。
        /// </summary>
        /// <returns>受影响的元素数。</returns>
        long RemoveRange();

        /// <summary>
        /// 基于当前查询获取元素，并设置一个新的元素。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <param name="returnNewDocument">如果为 <see langword="true"/> 值，则表示返回更新后的元素。</param>
        /// <returns>如果 <paramref name="returnNewDocument"/> 为 <see langword="true"/> 值，则返回更新后的文档，否则返回更新前的文档。 </returns>
        /// <returns>元素。</returns>
        TElement GetAndSet(TEntity entity, bool returnNewDocument = false);
        /// <summary>
        /// 基于当前查询获取旧元素，并修改为新元素。
        /// </summary>
        /// <param name="modifyExpression">修改表达式。</param>
        /// <param name="returnNewDocument">如果为 <see langword="true"/> 值，则表示返回更新后的元素。</param>
        /// <returns>如果 <paramref name="returnNewDocument"/> 为 <see langword="true"/> 值，则返回更新后的文档，否则返回更新前的文档。 </returns>
        TElement GetAndModify(Action<IModifyExpression<TEntity>> modifyExpression, bool returnNewDocument = false);
        /// <summary>
        /// 基于当前查询获取旧元素，并修改为新元素。
        /// </summary>
        /// <param name="entity">修改的实体。</param>
        /// <param name="returnNewDocument">如果为 <see langword="true"/> 值，则表示返回更新后的元素。</param>
        /// <returns>如果 <paramref name="returnNewDocument"/> 为 <see langword="true"/> 值，则返回更新后的文档，否则返回更新前的文档。 </returns>
        TElement GetAndModify(object entity, bool returnNewDocument = false);
        /// <summary>
        /// 基于当前查询获取并删除元素。
        /// </summary>
        /// <returns>被删除的元素。</returns>
        TElement GetAndRemove();



        /// <summary>
        /// 基于当前查询的修改操作。
        /// </summary>
        /// <param name="modifyExpression">修改的表达式。</param>
        /// <param name="upsert">不存在时是否执行插入操作。</param>
        /// <returns>受影响的元素数。</returns>
        Task<long> ModifyAsync(Action<IModifyExpression<TEntity>> modifyExpression, bool upsert = false);
        /// <summary>
        /// 基于当前查询的修改操作。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <param name="upsert">不存在时是否执行插入操作。</param>
        /// <returns>受影响的元素数。</returns>
        Task<long> ModifyAsync(object entity, bool upsert = false);
        /// <summary>
        /// 基于当前查询的批量修改。
        /// </summary>
        /// <param name="modifyExpression">修改的表达式。</param>
        /// <param name="upsert">不存在时是否执行插入操作。</param>
        /// <returns>受影响的元素数。</returns>
        Task<long> ModifyRangeAsync(Action<IModifyExpression<TEntity>> modifyExpression, bool upsert = false);

        /// <summary>
        /// 基于当前查询获取旧元素，并修改为新元素。
        /// </summary>
        /// <param name="entity">修改的实体。</param>
        /// <param name="upsert">不存在时是否执行插入操作。</param>
        /// <returns>受影响的元素数。</returns>
        Task<long> ModifyRangeAsync(object entity, bool upsert = false);

        /// <summary>
        /// 基于当前查询的删除操作。
        /// </summary>
        /// <returns>受影响的元素数。</returns>
        Task<long> RemoveAsync();
        /// <summary>
        /// 基于当前查询的批量删除操作。
        /// </summary>
        /// <returns>受影响的元素数。</returns>
        Task<long> RemoveRangeAsync();

        /// <summary>
        /// 基于当前查询获取元素，并设置一个新的元素。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <param name="returnNewDocument">如果为 <see langword="true"/> 值，则表示返回更新后的元素。</param>
        /// <returns>如果 <paramref name="returnNewDocument"/> 为 <see langword="true"/> 值，则返回更新后的文档，否则返回更新前的文档。 </returns>
        /// <returns>元素。</returns>
        Task<TElement> GetAndSetAsync(TEntity entity, bool returnNewDocument = false);
        /// <summary>
        /// 基于当前查询获取旧元素，并修改为新元素。
        /// </summary>
        /// <param name="modifyExpression">修改表达式。</param>
        /// <param name="returnNewDocument">如果为 <see langword="true"/> 值，则表示返回更新后的元素。</param>
        /// <returns>如果 <paramref name="returnNewDocument"/> 为 <see langword="true"/> 值，则返回更新后的文档，否则返回更新前的文档。 </returns>
        Task<TElement> GetAndModifyAsync(Action<IModifyExpression<TEntity>> modifyExpression, bool returnNewDocument = false);
        /// <summary>
        /// 基于当前查询获取旧元素，并修改为新元素。
        /// </summary>
        /// <param name="entity">修改的实体。</param>
        /// <param name="returnNewDocument">如果为 <see langword="true"/> 值，则表示返回更新后的元素。</param>
        /// <returns>如果 <paramref name="returnNewDocument"/> 为 <see langword="true"/> 值，则返回更新后的文档，否则返回更新前的文档。 </returns>
        Task<TElement> GetAndModifyAsync(object entity, bool returnNewDocument = false);
        /// <summary>
        /// 基于当前查询获取并删除元素。
        /// </summary>
        /// <returns>被删除的元素。</returns>
        Task<TElement> GetAndRemoveAsync();
    }
}
