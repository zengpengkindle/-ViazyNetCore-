using System.Collections.Generic;
using System.Linq.Expressions;

using MongoDB.Driver;

namespace System.Repos
{
    class MongoDBModifyExpression<TEntity> : IModifyExpression<TEntity>
    {
        public readonly List<UpdateDefinition<TEntity>> _definitions;

        public MongoDBModifyExpression()
        {
            this._definitions = new List<UpdateDefinition<TEntity>>();
        }

        public UpdateDefinition<TEntity> Definition => Builders<TEntity>.Update.Combine(this._definitions);

        private IModifyExpression<TEntity> AddDefinition(UpdateDefinition<TEntity> updateDefinition)
        {
            this._definitions.Add(updateDefinition);
            return this;
        }

        public IModifyExpression<TEntity> AddToSet<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, TItem value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.AddToSet(field, value));
        }

        public IModifyExpression<TEntity> AddToSetEach<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, IEnumerable<TItem> values)
        {
            return this.AddDefinition(Builders<TEntity>.Update.AddToSetEach(field, values));
        }

        public IModifyExpression<TEntity> BitAnd<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.BitwiseAnd(field, value));
        }

        public IModifyExpression<TEntity> BitOr<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.BitwiseOr(field, value));
        }

        public IModifyExpression<TEntity> BitXor<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.BitwiseXor(field, value));
        }

        public IModifyExpression<TEntity> Increment<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.Inc(field, value));
        }

        public IModifyExpression<TEntity> Max<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.Max(field, value));
        }

        public IModifyExpression<TEntity> Min<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.Min(field, value));
        }

        public IModifyExpression<TEntity> Multiply<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.Mul(field, value));
        }

        public IModifyExpression<TEntity> Now(Expression<Func<TEntity, DateTime>> field)
        {
            return this.AddDefinition(Builders<TEntity>.Update.CurrentDate(new ExpressionFieldDefinition<TEntity, DateTime>(field)));
        }
        public IModifyExpression<TEntity> Now(Expression<Func<TEntity, DateTime?>> field)
        {
            return this.AddDefinition(Builders<TEntity>.Update.CurrentDate(new ExpressionFieldDefinition<TEntity, DateTime?>(field)));
        }

        public IModifyExpression<TEntity> PopFirst<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field)
        {

            return this.AddDefinition(Builders<TEntity>.Update.PopFirst(new ExpressionFieldDefinition<TEntity, IEnumerable<TItem>>(field)));
        }

        public IModifyExpression<TEntity> PopLast<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field)
        {
            return this.AddDefinition(Builders<TEntity>.Update.PopLast(new ExpressionFieldDefinition<TEntity, IEnumerable<TItem>>(field)));
        }

        public IModifyExpression<TEntity> Pull<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, TItem value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.Pull(field, value));
        }

        public IModifyExpression<TEntity> PullAll<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, IEnumerable<TItem> values)
        {
            return this.AddDefinition(Builders<TEntity>.Update.PullAll(field, values));
        }

        public IModifyExpression<TEntity> PullFilter<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, Expression<Func<TItem, bool>> filter)
        {
            return this.AddDefinition(Builders<TEntity>.Update.PullFilter(field, filter));
        }

        public IModifyExpression<TEntity> Push<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, TItem value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.Push(field, value));
        }

        public IModifyExpression<TEntity> PushEach<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, IEnumerable<TItem> values, int? slice = null, int? position = null, Expression<Func<TEntity, TItem>>? sort = null, bool descending = false)
        {
            SortDefinition<TItem>? sortDefinition = null;
            if(sort != null)
            {
                var sortField = new ExpressionFieldDefinition<TItem>(sort);
                sortDefinition = descending
                    ? Builders<TItem>.Sort.Descending(sortField)
                    : Builders<TItem>.Sort.Ascending(sortField);
            }
            return this.AddDefinition(Builders<TEntity>.Update.PushEach(
                new ExpressionFieldDefinition<TEntity, IEnumerable<TItem>>(field), values, slice, position, sortDefinition));
        }

        public IModifyExpression<TEntity> Rename<TItem>(Expression<Func<TEntity, TItem>> field, string newName)
        {
            return this.AddDefinition(Builders<TEntity>.Update.Rename(new ExpressionFieldDefinition<TEntity, TItem>(field), newName));
        }

        public IModifyExpression<TEntity> Set<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.Set(field, value));
        }

        public IModifyExpression<TEntity> SetOnInsert<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            return this.AddDefinition(Builders<TEntity>.Update.SetOnInsert(field, value));
        }

        public IModifyExpression<TEntity> Unset<TItem>(Expression<Func<TEntity, TItem>> field)
        {
            return this.AddDefinition(Builders<TEntity>.Update.Unset(new ExpressionFieldDefinition<TEntity, TItem>(field)));
        }
    }
}
