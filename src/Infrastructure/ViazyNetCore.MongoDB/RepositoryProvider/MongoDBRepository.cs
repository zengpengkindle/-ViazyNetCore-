using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace System.Repos
{
    /// <summary>
    /// 定义一个基于 MongoDB 的仓储。
    /// </summary>
    /// <typeparam name="TEntity">实体的数据类型。</typeparam>
    /// <typeparam name="TElement">元素的数据类型。</typeparam>
    public interface IMongoDBRepository<TEntity, TElement> : IRepository<TEntity, TElement>
    {
        /// <summary>
        /// 获取 MongoDB 集合。
        /// </summary>
        IMongoCollection<TEntity> Collection { get; }
        /// <summary>
        /// 获取 MangoDB 可查询集合。
        /// </summary>
        new IMongoQueryable<TElement> Queryable { get; }
    }

    class EntityHelper<TEntity>
    {
        public static UpdateDefinition<TEntity> Set<T>(string name, T value)
        {
            return Builders<TEntity>.Update.Set(name, value);
        }

        public static readonly MethodInfo UpdateSetMethod = typeof(EntityHelper<TEntity>).GetMethod("Set").MustBe();

    }
    /// <summary>
    /// 表示一个基于 MongoDB 的仓储。
    /// </summary>
    /// <typeparam name="TEntity">实体的数据类型。</typeparam>
    /// <typeparam name="TElement">元素的数据类型。</typeparam>
    public class MongoDBRepository<TEntity, TElement> : IMongoDBRepository<TEntity, TElement>
    {
        private readonly IMongoCollection<TEntity> _collection;

        /// <summary>
        /// 初始化一个 <see cref="MongoDBRepository{TEntity, TElement}"/> 类的新实例。
        /// </summary>
        /// <param name="collection">一个 MongoDB 集合。</param>
        /// <param name="queryable">一个MangoDB 可查询集合。</param>
        public MongoDBRepository(IMongoCollection<TEntity> collection, IMongoQueryable<TElement> queryable)
        {
            this._collection = collection;
            this.Queryable = queryable ?? throw new ArgumentNullException(nameof(queryable));
        }

        /// <inheritdoc />
        public IMongoCollection<TEntity> Collection => this._collection ?? throw new NotSupportedException($"The type '{typeof(TEntity).FullName}' is not a valid repository type.");

        /// <inheritdoc />
        public IMongoQueryable<TElement> Queryable { get; }

        IQueryable<TElement> IRepositoryQuery<TElement>.Queryable => this.Queryable;

        /// <inheritdoc />
        public IRepositoryQuery<TEntity, TOutput> CreateQuery<TOutput>(IQueryable<TOutput> queryable)
        {
            return new MongoDBRepository<TEntity, TOutput>(this._collection, queryable.MustBe<IMongoQueryable<TOutput>>());
        }

        IRepositoryQuery<TOutput> IRepositoryQuery<TElement>.CreateQuery<TOutput>(IQueryable<TOutput> queryable)
        {
            return this.CreateQuery(queryable);
        }

        /// <inheritdoc />
        public IRepositoryQuery<TEntity, TElement> Random(long count)
        {
            return new MongoDBRepository<TEntity, TElement>(this.Collection, this.Queryable.Sample(count));
        }

        private UpdateDefinition<TEntity> CreateUpdate(Action<IModifyExpression<TEntity>> modifyExpression)
        {
            var builder = new MongoDBModifyExpression<TEntity>();
            modifyExpression(builder);
            return builder.Definition;
        }

        private IEnumerable<ExpressionFilterDefinition<TEntity>> VisitWhere(Expression expression)
        {
            if(expression.NodeType == ExpressionType.Call)
            {
                var callExp = expression.MustBe<MethodCallExpression>();
                if(callExp.Method.DeclaringType == typeof(Queryable))
                {
                    foreach(var predicate in this.VisitWhere(callExp.Arguments[0]))
                    {
                        yield return predicate;
                    }

                    if(callExp.Method.Name == nameof(Linq.Queryable.Where))
                    {
                        foreach(var predicate in this.VisitWhere(callExp.Arguments[1]))
                        {
                            yield return predicate;
                        }
                    }
                }
            }
            else if(expression.NodeType == ExpressionType.Quote)
            {
                var exp = expression.MustBe<UnaryExpression>();

                foreach(var predicate in this.VisitWhere(exp.Operand))
                {
                    yield return predicate;
                }
            }
            else if(expression.NodeType == ExpressionType.Lambda)
            {
                if(expression is Expression<Func<TEntity, bool>> exp)
                {
                    yield return new ExpressionFilterDefinition<TEntity>(exp);
                }
            }
        }

        private FilterDefinition<TEntity> CreateWhere()
        {
            var predicates = this.VisitWhere(this.Queryable.Expression);

            var filter = Builders<TEntity>.Filter.And(predicates);
            return filter;

        }

        private ProjectionDefinition<TEntity, TElement>? VisitSelect(Expression expression)
        {
            if(expression.NodeType == ExpressionType.Call)
            {
                var callExp = expression.MustBe<MethodCallExpression>();
                if(callExp.Method.DeclaringType == typeof(Queryable))
                {
                    if(callExp.Method.Name == nameof(Linq.Queryable.Select))
                    {
                        return this.VisitSelect(callExp.Arguments[1]);
                    }
                    return this.VisitSelect(callExp.Arguments[0]);
                }
            }
            else if(expression.NodeType == ExpressionType.Quote)
            {
                var exp = expression.MustBe<UnaryExpression>();

                return this.VisitSelect(exp.Operand);
            }
            else if(expression.NodeType == ExpressionType.Lambda)
            {
                if(expression is Expression<Func<TEntity, TElement>> exp)
                {
                    return Builders<TEntity>.Projection.Expression(exp);
                }
            }

            return null;
        }

        private ProjectionDefinition<TEntity, TElement>? CreateSelect()
        {
            return this.VisitSelect(this.Queryable.Expression);
        }

        private IEnumerable<SortDefinition<TEntity>> VisitSort(Expression expression, bool desc = false)
        {
            if(expression.NodeType == ExpressionType.Call)
            {
                var callExp = expression.MustBe<MethodCallExpression>();
                if(callExp.Method.DeclaringType == typeof(Queryable))
                {
                    foreach(var definition in this.VisitSort(callExp.Arguments[0], desc))
                    {
                        yield return definition;
                    }

                    if(callExp.Method.Name == nameof(Linq.Queryable.OrderBy))
                    {
                        foreach(var definition in this.VisitSort(callExp.Arguments[1], false))
                        {
                            yield return definition;
                        }
                    }

                    else if(callExp.Method.Name == nameof(Linq.Queryable.OrderByDescending))
                    {
                        foreach(var definition in this.VisitSort(callExp.Arguments[1], true))
                        {
                            yield return definition;
                        }
                    }
                }
            }
            else if(expression.NodeType == ExpressionType.Quote)
            {
                var exp = expression.MustBe<UnaryExpression>();

                foreach(var definition in this.VisitSort(exp.Operand, desc))
                {
                    yield return definition;
                }
            }
            else if(expression.NodeType == ExpressionType.Lambda)
            {
                if(expression is LambdaExpression exp)
                {
                    if(desc) yield return Builders<TEntity>.Sort.Descending(new ExpressionFieldDefinition<TEntity>(exp));
                    else yield return Builders<TEntity>.Sort.Ascending(new ExpressionFieldDefinition<TEntity>(exp));
                }
            }
        }

        private SortDefinition<TEntity>? CreateSort()
        {
            var sortDefinitions = this.VisitSort(this.Queryable.Expression).ToArray();

            if(sortDefinitions.Length == 0) return null;

            return Builders<TEntity>.Sort.Combine(sortDefinitions);
        }

        private UpdateDefinition<TEntity> CreateUpdate(object entity)
        {
            var mapper = TypeMapper<TEntity>.Instance;
            var setters = new List<UpdateDefinition<TEntity>>();
            foreach(var property in TypeMapper.Create(entity.GetType()).Properties)
            {
                var op = mapper[property.Name];
                if(op is null) throw new MissingMemberException(typeof(TEntity).Name, property.Name);
                var handler = EntityHelper<TEntity>.UpdateSetMethod.MakeGenericMethod(op.Property.PropertyType).CreateMethodInvoker();
                setters.Add((UpdateDefinition<TEntity>)handler(null, op.Name, property.GetValue(entity)));
            }
            return Builders<TEntity>.Update.Combine(setters);
        }

        /// <inheritdoc />
        public void Add(TEntity entity)
        {
            this.Collection.InsertOne(entity);
        }

        /// <inheritdoc />
        public Task AddAsync(TEntity entity)
        {
            return this.Collection.InsertOneAsync(entity);
        }

        /// <inheritdoc />
        public void AddRange(IEnumerable<TEntity> entities)
        {
            this.Collection.InsertMany(entities);
        }

        /// <inheritdoc />
        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return this.Collection.InsertManyAsync(entities);
        }

        /// <inheritdoc />
        public long Modify(object entity, bool upsert = false)
        {
            var setters = new Dictionary<string, object?>();
            var mapper = TypeMapper<TEntity>.Instance;

            foreach(var property in TypeMapper.Create(entity.GetType()).Properties)
            {
                var op = mapper[property.Name];
                if(op is null) throw new MissingMemberException(typeof(TEntity).Name, property.Name);
                //- 主键不参与修改
                if(op.IsKey) continue;
                setters.Add(op.Name, op.GetValue(entity));
            }

            return this.Collection.UpdateOne(this.CreateWhere(), new BsonDocument("$set", new BsonDocument(setters)), new UpdateOptions
            {
                IsUpsert = upsert,
            }).ModifiedCount;
        }


        /// <inheritdoc />
        public async Task<long> ModifyAsync(object entity, bool upsert = false)
        {
            var r = await this.Collection.UpdateOneAsync(this.CreateWhere(), this.CreateUpdate(entity), new UpdateOptions
            {
                IsUpsert = upsert,
            }).ConfigureAwait(false);
            return r.ModifiedCount;
        }

        /// <inheritdoc />
        public long Modify(Action<IModifyExpression<TEntity>> modifyExpression, bool upsert = false)
        {
            return this.Collection.UpdateOne(this.CreateWhere(), this.CreateUpdate(modifyExpression), new UpdateOptions
            {
                IsUpsert = upsert,
            }).ModifiedCount;
        }

        /// <inheritdoc />
        public async Task<long> ModifyAsync(Action<IModifyExpression<TEntity>> modifyExpression, bool upsert = false)
        {
            var r = await this.Collection.UpdateOneAsync(this.CreateWhere(), this.CreateUpdate(modifyExpression), new UpdateOptions
            {
                IsUpsert = upsert,
            }).ConfigureAwait(false);
            return r.ModifiedCount;
        }

        /// <inheritdoc />
        public long ModifyRange(Action<IModifyExpression<TEntity>> modifyExpression, bool upsert = false)
        {
            return this.Collection.UpdateMany(this.CreateWhere(), this.CreateUpdate(modifyExpression), new UpdateOptions
            {
                IsUpsert = upsert,
            }).ModifiedCount;
        }

        /// <inheritdoc />
        public long ModifyRange(object entity, bool upsert = false)
        {
            return this.Collection.UpdateMany(this.CreateWhere(), this.CreateUpdate(entity), new UpdateOptions
            {
                IsUpsert = upsert,
            }).ModifiedCount;
        }

        /// <inheritdoc />
        public async Task<long> ModifyRangeAsync(Action<IModifyExpression<TEntity>> modifyExpression, bool upsert = false)
        {
            var r = await this.Collection.UpdateManyAsync(this.CreateWhere(), this.CreateUpdate(modifyExpression), new UpdateOptions
            {
                IsUpsert = upsert,
            }).ConfigureAwait(false);
            return r.ModifiedCount;
        }

        /// <inheritdoc />
        public async Task<long> ModifyRangeAsync(object entity, bool upsert = false)
        {
            var r = await this.Collection.UpdateManyAsync(this.CreateWhere(), this.CreateUpdate(entity), new UpdateOptions
            {
                IsUpsert = upsert,
            }).ConfigureAwait(false);
            return r.ModifiedCount;
        }

        /// <inheritdoc />
        public long Remove()
        {
            return this.Collection.DeleteOne(this.CreateWhere()).DeletedCount;
        }

        /// <inheritdoc />
        public async Task<long> RemoveAsync()
        {
            var r = await this.Collection.DeleteOneAsync(this.CreateWhere()).ConfigureAwait(false);
            return r.DeletedCount;
        }

        /// <inheritdoc />
        public long RemoveRange()
        {
            return this.Collection.DeleteMany(this.CreateWhere()).DeletedCount;
        }

        /// <inheritdoc />
        public async Task<long> RemoveRangeAsync()
        {
            var r = await this.Collection.DeleteManyAsync(this.CreateWhere()).ConfigureAwait(false);
            return r.DeletedCount;
        }

        /// <inheritdoc />
        public TElement GetAndSet(TEntity entity, bool returnNewDocument = false)
        {
            var options = new FindOneAndReplaceOptions<TEntity, TElement>()
            {
                Projection = this.CreateSelect(),
                Sort = this.CreateSort(),
                ReturnDocument = returnNewDocument ? ReturnDocument.After : ReturnDocument.Before,
            };

            return this.Collection.FindOneAndReplace(this.CreateWhere(), entity, options);
        }

        /// <inheritdoc />
        public Task<TElement> GetAndSetAsync(TEntity entity, bool returnNewDocument = false)
        {
            var options = new FindOneAndReplaceOptions<TEntity, TElement>()
            {
                Projection = this.CreateSelect(),
                Sort = this.CreateSort(),
                ReturnDocument = returnNewDocument ? ReturnDocument.After : ReturnDocument.Before,
            };

            return this.Collection.FindOneAndReplaceAsync(this.CreateWhere(), entity, options);
        }

        /// <inheritdoc />
        public TElement GetAndModify(Action<IModifyExpression<TEntity>> modifyExpression, bool returnNewDocument = false)
        {
            var options = new FindOneAndUpdateOptions<TEntity, TElement>()
            {
                Projection = this.CreateSelect(),
                Sort = this.CreateSort(),
                ReturnDocument = returnNewDocument ? ReturnDocument.After : ReturnDocument.Before,
            };

            var update = this.CreateUpdate(modifyExpression);

            return this.Collection.FindOneAndUpdate(this.CreateWhere(), update, options);
        }

        /// <inheritdoc />
        public TElement GetAndModify(object entity, bool returnNewDocument = false)
        {
            var options = new FindOneAndUpdateOptions<TEntity, TElement>()
            {
                Projection = this.CreateSelect(),
                Sort = this.CreateSort(),
                ReturnDocument = returnNewDocument ? ReturnDocument.After : ReturnDocument.Before,
            };

            var update = this.CreateUpdate(entity);

            return this.Collection.FindOneAndUpdate(this.CreateWhere(), update, options);
        }

        /// <inheritdoc />
        public Task<TElement> GetAndModifyAsync(Action<IModifyExpression<TEntity>> modifyExpression, bool returnNewDocument = false)
        {
            var options = new FindOneAndUpdateOptions<TEntity, TElement>()
            {
                Projection = this.CreateSelect(),
                Sort = this.CreateSort(),
                ReturnDocument = returnNewDocument ? ReturnDocument.After : ReturnDocument.Before,
            };

            var update = this.CreateUpdate(modifyExpression);

            return this.Collection.FindOneAndUpdateAsync(this.CreateWhere(), update, options);
        }

        /// <inheritdoc />
        public Task<TElement> GetAndModifyAsync(object entity, bool returnNewDocument = false)
        {
            var options = new FindOneAndUpdateOptions<TEntity, TElement>()
            {
                Projection = this.CreateSelect(),
                Sort = this.CreateSort(),
                ReturnDocument = returnNewDocument ? ReturnDocument.After : ReturnDocument.Before,
            };

            var update = this.CreateUpdate(entity);

            return this.Collection.FindOneAndUpdateAsync(this.CreateWhere(), update, options);
        }

        /// <inheritdoc />
        public TElement GetAndRemove()
        {
            var options = new FindOneAndDeleteOptions<TEntity, TElement>()
            {
                Projection = this.CreateSelect(),
                Sort = this.CreateSort()
            };

            return this.Collection.FindOneAndDelete(this.CreateWhere(), options);
        }

        /// <inheritdoc />
        public Task<TElement> GetAndRemoveAsync()
        {
            var options = new FindOneAndDeleteOptions<TEntity, TElement>()
            {
                Projection = this.CreateSelect(),
                Sort = this.CreateSort()
            };

            return this.Collection.FindOneAndDeleteAsync(this.CreateWhere(), options);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Queryable?.ToString() ?? string.Empty;
        }

        /// <inheritdoc />
        public PageData<TElement> ToPage(int pageNumber = 1, int pageSize = 10)
        {
            if(pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if(pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));

            var total = this.LongCount();
            var rows = this.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
            return new PageData<TElement>(rows, total);
        }

        /// <inheritdoc />
        public async Task<PageData<TElement>> ToPageAsync(int pageNumber = 1, int pageSize = 10)
        {
            if(pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if(pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));

            var total = this.LongCount();
            var rows = await this.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArrayAsync().ConfigureAwait(false);
            return new PageData<TElement>(rows, total);
        }



        //public IEnumerator<TElement> GetEnumerator() => this.Queryable.GetEnumerator();

        //IEnumerator IEnumerable.GetEnumerator() => (this.Queryable as IEnumerable).GetEnumerator();
    }
}
