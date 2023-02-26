
using MongoDB.Bson;

namespace System.Repos
{
    /// <summary>
    /// 表示一个仓储的服务基类。
    /// </summary>
    [Injection]
    public abstract class RepositoryServiceBase
    {
        /// <summary>
        /// 获取仓储提供程序。
        /// </summary>
        public IRepositoryProvider RepositoryProvider { get; }

        /// <summary>
        /// 初始化一个 <see cref="RepositoryServiceBase"/> 类的新实例。
        /// </summary>
        /// <param name="repositoryProvider">仓储提供程序。</param>
        protected RepositoryServiceBase(IRepositoryProvider repositoryProvider)
        {
            this.RepositoryProvider = repositoryProvider;
        }

        /// <summary>
        /// 创建一个读写仓储。
        /// </summary>
        /// <typeparam name="TRecord">实体的数据类型。</typeparam>
        /// <param name="nameBuilder">集合的名称生成器。</param>
        /// <returns>基于实体的读写仓储。</returns>
        protected virtual IRepository<TRecord, TRecord> CreateRepository<TRecord>(Func<TypeMapper, string>? nameBuilder = null) => this.RepositoryProvider.Create<TRecord>(nameBuilder);

        /// <summary>
        /// 创建一个只读仓储。
        /// </summary>
        /// <typeparam name="TRecord">实体的数据类型。</typeparam>
        /// <param name="nameBuilder">集合的名称生成器。</param>
        /// <returns>基于实体的只读仓储。</returns>
        protected virtual IRepositoryQuery<TRecord, TRecord> CreateQuery<TRecord>(Func<TypeMapper, string>? nameBuilder = null) => this.CreateRepository<TRecord>(nameBuilder);

        /// <summary>
        /// 获取一个新的随机编号。
        /// </summary>
        /// <returns>随机编号。</returns>
        protected virtual string NewId() => ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// 表示一个类型化仓储的服务基类。
    /// </summary>
    /// <typeparam name="TRecord">实体的数据类型。</typeparam>
    public abstract class RepositoryServiceBase<TRecord> : RepositoryServiceBase
    {
        /// <summary>
        /// 初始化一个 <see cref="RepositoryServiceBase{TRecord}"/> 类的新实例。
        /// </summary>
        /// <param name="repositoryProvider">仓储提供程序。</param>
        protected RepositoryServiceBase(IRepositoryProvider repositoryProvider) : base(repositoryProvider) { }

        /// <summary>
        /// 创建一个读写仓储。
        /// </summary>
        /// <param name="nameBuilder">集合的名称生成器。</param>
        /// <returns>基于实体的读写仓储。</returns>
        protected virtual IRepository<TRecord, TRecord> CreateRepository(Func<TypeMapper, string>? nameBuilder = null) => this.RepositoryProvider.Create<TRecord>(nameBuilder);

        /// <summary>
        /// 创建一个只读仓储。
        /// </summary>
        /// <param name="nameBuilder">集合的名称生成器。</param>
        /// <returns>基于实体的只读仓储。</returns>
        protected virtual IRepositoryQuery<TRecord, TRecord> CreateQuery(Func<TypeMapper, string>? nameBuilder = null) => this.CreateRepository(nameBuilder);
    }
}
