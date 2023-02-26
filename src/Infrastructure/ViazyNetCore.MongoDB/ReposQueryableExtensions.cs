using System.Collections.Generic;
using System.Linq.Expressions;
using System.Repos;

namespace System.Linq
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public static class ReposQueryableExtensions
    {
        public static IRepositoryQuery<TEntity, TSource> DefaultIfEmpty<TEntity, TSource>(this IRepositoryQuery<TEntity, TSource> source)
        {
            return source.CreateQuery(source.Queryable.DefaultIfEmpty());
        }

        public static IRepositoryQuery<TEntity, TSource> DefaultIfEmpty<TEntity, TSource>(this IRepositoryQuery<TEntity, TSource> source, TSource defaultValue)
        {
            return source.CreateQuery(source.Queryable.DefaultIfEmpty(defaultValue));
        }

        public static IRepositoryQuery<TEntity, TSource> Distinct<TEntity, TSource>(this IRepositoryQuery<TEntity, TSource> source)
        {
            return source.CreateQuery(source.Queryable.Distinct());
        }

        public static IRepositoryQuery<TResult> GroupBy<TSource, TKey, TResult>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector)
        {
            return source.CreateQuery(source.Queryable.GroupBy(keySelector, resultSelector));
        }

        public static IRepositoryQuery<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        {
            return source.CreateQuery(source.Queryable.GroupBy(keySelector, elementSelector, resultSelector));
        }

        public static IRepositoryQuery<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
        {
            return source.CreateQuery(source.Queryable.GroupBy(keySelector, elementSelector));
        }

        public static IRepositoryQuery<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return source.CreateQuery(source.Queryable.GroupBy(keySelector));
        }

        public static IRepositoryQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IRepositoryQuery<TOuter> outer, IRepositoryQuery<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
        {
            return outer.CreateQuery(outer.Queryable.GroupJoin(inner.Queryable, outerKeySelector, innerKeySelector, resultSelector));
        }

        public static IRepositoryQuery<TResult> Join<TOuter, TInner, TKey, TResult>(this IRepositoryQuery<TOuter> outer, IRepositoryQuery<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return outer.CreateQuery(outer.Queryable.Join(inner.Queryable, outerKeySelector, innerKeySelector, resultSelector));
        }


        //public static IRepositoryQuery<TEntity, TSource> InnerJoin<TSource>(this IRepositoryQuery<TEntity, TSource> source, Expression<Func<TSource, bool>> predicate)
        //{
        //    //from t0 in table0
        //    //from t1 in table1.InnerJoin(t1 => t0.Id == t1.RoleId)


        //    //predicate.Body
        //}

        public static IRepositoryQuery<TSource> OrderBy<TSource, TKey>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return source.CreateQuery(source.Queryable.OrderBy(keySelector));
        }

        public static IRepositoryQuery<TSource> OrderByDescending<TSource, TKey>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return source.CreateQuery(source.Queryable.OrderByDescending(keySelector));
        }

        public static IRepositoryQuery<TResult> Select<TSource, TResult>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return source.CreateQuery(source.Queryable.Select(selector));
        }

        public static IRepositoryQuery<TResult> SelectMany<TSource, TCollection, TResult>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            return source.CreateQuery(source.Queryable.SelectMany(collectionSelector, resultSelector));
        }

        public static IRepositoryQuery<TResult> SelectMany<TSource, TResult>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
        {
            return source.CreateQuery(source.Queryable.SelectMany(selector));
        }

        public static IRepositoryQuery<TSource> Skip<TSource>(this IRepositoryQuery<TSource> source, int count)
        {
            return source.CreateQuery(source.Queryable.Skip(count));
        }

        public static IRepositoryQuery<TSource> Take<TSource>(this IRepositoryQuery<TSource> source, int count)
        {
            return source.CreateQuery(source.Queryable.Take(count));
        }


        public static IRepositoryQuery<TSource> Where<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.CreateQuery(source.Queryable.Where(predicate));
        }

        public static IRepositoryQuery<TEntity, TSource> Where<TEntity, TSource>(this IRepositoryQuery<TEntity, TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return source.CreateQuery(source.Queryable.Where(predicate));
        }

        public static IRepositoryQuery<TEntity, TSource> AsQueryable<TEntity, TSource>(this IRepositoryQuery<TEntity, TSource> source) => source;
    }

#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
