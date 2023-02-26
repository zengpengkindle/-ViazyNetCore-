using System.Collections.Generic;
using System.Linq.Expressions;

namespace System
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public interface IModifyExpression<TEntity>
    {
        IModifyExpression<TEntity> AddToSet<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, TItem value);
        IModifyExpression<TEntity> AddToSetEach<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, IEnumerable<TItem> values);

        IModifyExpression<TEntity> BitAnd<TField>(Expression<Func<TEntity, TField>> field, TField value);
        IModifyExpression<TEntity> BitOr<TField>(Expression<Func<TEntity, TField>> field, TField value);
        IModifyExpression<TEntity> BitXor<TField>(Expression<Func<TEntity, TField>> field, TField value);

        IModifyExpression<TEntity> Now(Expression<Func<TEntity, DateTime>> field);
        IModifyExpression<TEntity> Now(Expression<Func<TEntity, DateTime?>> field);

        IModifyExpression<TEntity> Increment<TField>(Expression<Func<TEntity, TField>> field, TField value);
        IModifyExpression<TEntity> Max<TField>(Expression<Func<TEntity, TField>> field, TField value);
        IModifyExpression<TEntity> Min<TField>(Expression<Func<TEntity, TField>> field, TField value);
        IModifyExpression<TEntity> Multiply<TField>(Expression<Func<TEntity, TField>> field, TField value);

        IModifyExpression<TEntity> PopFirst<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field);
        IModifyExpression<TEntity> PopLast<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field);
        IModifyExpression<TEntity> Pull<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, TItem value);
        IModifyExpression<TEntity> PullAll<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, IEnumerable<TItem> values);
        IModifyExpression<TEntity> PullFilter<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, Expression<Func<TItem, bool>> filter);
        IModifyExpression<TEntity> Push<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, TItem value);
        IModifyExpression<TEntity> PushEach<TItem>(Expression<Func<TEntity, IEnumerable<TItem>>> field, IEnumerable<TItem> values, int? slice = null, int? position = null, Expression<Func<TEntity, TItem>>? sort = null, bool descending = false);
   
        IModifyExpression<TEntity> Set<TField>(Expression<Func<TEntity, TField>> field, TField value);
        IModifyExpression<TEntity> SetOnInsert<TField>(Expression<Func<TEntity, TField>> field, TField value);

        IModifyExpression<TEntity> Unset<TItem>(Expression<Func<TEntity, TItem>> field);
        IModifyExpression<TEntity> Rename<TItem>(Expression<Func<TEntity, TItem>> field, string newName);
    }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
