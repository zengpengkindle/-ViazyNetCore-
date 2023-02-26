using System.Collections.Generic;
using System.Linq;

using MongoDB.Bson;
using MongoDB.Driver;

namespace System.Repos
{
    /// <summary>
    /// 表示一个 MongoDB 的扩展库。
    /// </summary>
    public static class MongoDatabaseExtensions
    {
        private static readonly Collections.Concurrent.ConcurrentDictionary<string, bool> EntityStatusCache
            = new(StringComparer.OrdinalIgnoreCase);

        static MongoDatabaseExtensions()
        {
            MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(typeof(DateTime)
                , new MongoDB.Bson.Serialization.Serializers.DateTimeSerializer(DateTimeKind.Local));
            MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(typeof(decimal)
                , new MongoDB.Bson.Serialization.Serializers.DecimalSerializer(BsonType.Double));
            MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(new MongoDB.Bson.Serialization.Serializers.DateTimeOffsetSerializer(BsonType.String));
        }

        /// <summary>
        /// 获取指定实体类型的仓储。
        /// </summary>
        /// <typeparam name="TEntity">实体。</typeparam>
        /// <param name="mongoDatabase">数据库。</param>
        /// <param name="nameBuilder">集合的名称生成器。</param>
        /// <returns>实体类型的仓储。</returns>
        public static IMongoDBRepository<TEntity, TEntity> GetRepository<TEntity>(this IMongoDatabase mongoDatabase, Func<TypeMapper, string>? nameBuilder = null)
        {
            var name = nameBuilder is null
                ? null
                : nameBuilder(TypeMapper<TEntity>.Instance);

            if(name.IsNull()) name = TypeMapper<TEntity>.Instance.Name + "Set";

            var collection = mongoDatabase.GetCollection<TEntity>(name);
            if(!IndexStatus<TEntity>.IsBuilded(name))
            {
                //- 第一次获取的时候对象的时候都会重新构建索引
                var indexes = IndexStatus<TEntity>.Indexes;
                if(indexes.Length > 0)
                {
                    using var session = mongoDatabase.Client.StartSession();
                    foreach(var index in indexes)
                    {
                        try
                        {
                            collection.Indexes.CreateOne(session, index);
                        }
                        catch(MongoCommandException mce) when(mce.Code == 85)
                        {
                            //- link:https://docs.mongodb.com/manual/reference/command/createIndexes/
                            //- 索引已存在，但是配置不同
                            collection.Indexes.DropOne(index.Options.Name);
                            collection.Indexes.CreateOne(session, index);
                        }
                    }

                }
            }

            return new MongoDBRepository<TEntity, TEntity>(collection, collection.AsQueryable());
        }
        //internal readonly static Collation CaseInsensitiveCollation = new Collation("en", strength: CollationStrength.Secondary);
        static class IndexStatus<TEntity>
        {
            public readonly static CreateIndexModel<TEntity>[] Indexes = BuildIndexes().ToArray();
            private static readonly string NamePrefix = typeof(TEntity).FullName + "#";

            private static IEnumerable<CreateIndexModel<TEntity>> BuildIndexes()
            {
                var builder = new IndexKeysDefinitionBuilder<TEntity>();
                var items = (from prop in TypeMapper<TEntity>.Instance.Properties
                             let index = prop.Property.GetAttribute<IRepositoryIndex>()
                             where index != null
                             orderby index.Id
                             select new { prop, index }).GroupBy(item => item.index.Id);
                foreach(var item in items)
                {
                    if(item.Key == -1)
                    {
                        foreach(var p in item)
                        {
                            var keysDefinition = p.index.Descending ? builder.Descending(p.prop.Name) : builder.Ascending(p.prop.Name);
                            if(p.index is not RepositoryIndexAttribute ria) throw new InvalidOperationException("Cannot found the primary index attribute.");
                            var options = ria.CreateIndexOptions();
                            options.Name = "Index_" + p.prop.Name;
                            yield return new CreateIndexModel<TEntity>(keysDefinition, options);
                        }
                    }
                    else
                    {
                        var keysDefinition = builder.Combine(item.Select(p => p.index.Descending ? builder.Descending(p.prop.Name) : builder.Ascending(p.prop.Name)));
                        var ria = item.Select(i => i.index).OfType<RepositoryIndexAttribute>().FirstOrDefault();
                        if(ria is null) throw new InvalidOperationException("Cannot found the primary index attribute.");
                        var options = ria.CreateIndexOptions();
                        options.Name = "CompoundIndex_" + item.Key;
                        yield return new CreateIndexModel<TEntity>(keysDefinition, options);
                    }
                }
            }

            public static bool IsBuilded(string name) => !EntityStatusCache.TryAdd(NamePrefix + name, true);
        }
    }
}
