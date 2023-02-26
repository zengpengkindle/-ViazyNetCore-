using System.Collections.Generic;
using System.Linq;

using MongoDB.Driver;

namespace System.Repos
{
    /// <summary>
    /// 表示一个仓储提供程序。
    /// </summary>
    public class MongoDBRepositoryProvider : IRepositoryProvider
    {
        private readonly IMongoDatabase _mongoDatabase;

        /// <summary>
        /// 初始化一个 <see cref="MongoDBRepositoryProvider"/> 类的新实例。
        /// </summary>
        /// <param name="mongoDatabase">数据库。</param>
        public MongoDBRepositoryProvider(IMongoDatabase mongoDatabase)
        {
            this._mongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase));
        }

        /// <inheritdoc />
        public virtual IRepository<TEntity, TEntity> Create<TEntity>(Func<TypeMapper, string>? nameBuilder = null)
        {
            return this._mongoDatabase.GetRepository<TEntity>(nameBuilder);
        }
    }

    /// <summary>
    /// 表示一个仓储管理器。
    /// </summary>
    public abstract class MongoDBRepositoryManagerBase : IRepositoryManager
    {
        private Dictionary<string, MongoDBRepositoryProvider>? _providers;

        /// <summary>
        /// 获取默认的仓储提供程序。
        /// </summary>
        public IRepositoryProvider Default => this.GetProvider(null);

        /// <inheritdoc />
        public virtual IRepositoryProvider GetProvider(string? database)
        {
            if(this._providers is null) throw new InvalidOperationException();
            if(database is null) return this._providers.First().Value;
            if(!this._providers.TryGetValue(database, out var provider))
                throw new InvalidOperationException($"The database '{database}' is not exists.");
            return provider;
        }

        /// <inheritdoc />
        public virtual void Initialize(IEnumerable<KeyValuePair<string, string>> connections)
        {
            if(connections is null)
            {
                throw new ArgumentNullException(nameof(connections));
            }
            this._providers = new Dictionary<string, MongoDBRepositoryProvider>(StringComparer.InvariantCulture);
            foreach(var item in connections)
            {
                var mongoClient = new MongoClient(item.Value);
                this._providers[item.Key] = new MongoDBRepositoryProvider(mongoClient.GetDatabase(item.Key));
            }
        }
    }

    /// <summary>
    /// 表示一个默认的仓储管理器。
    /// </summary>
    public class MongoDBRepositoryManager: MongoDBRepositoryManagerBase
    {

    }
}
