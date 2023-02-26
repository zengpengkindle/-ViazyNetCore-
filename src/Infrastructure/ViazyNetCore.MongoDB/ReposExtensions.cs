using System.Collections.Generic;
using System.Linq.Expressions;
using System.Repos;
using ViazyNetCore;

namespace System.Linq
{
#pragma warning disable CS1591,CS8619,CS8603 // 缺少对公共可见类型或成员的 XML 注释
    public static class ReposExtensions
    {
        public static bool All<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
            => source.Queryable.All(predicate);

        public static bool Any<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.Any();

        public static bool Any<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
            => source.Queryable.Any(predicate);

        public static double Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, double>> selector)
             => source.Queryable.Average(selector);

        public static float? Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, float?>> selector)
             => source.Queryable.Average(selector);

        public static double? Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, long?>> selector)
             => source.Queryable.Average(selector);

        public static double? Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, int?>> selector)
             => source.Queryable.Average(selector);

        public static double? Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, double?>> selector)
             => source.Queryable.Average(selector);

        public static decimal? Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, decimal?>> selector)
             => source.Queryable.Average(selector);

        public static double Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, long>> selector)
             => source.Queryable.Average(selector);

        public static double Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, int>> selector)
             => source.Queryable.Average(selector);

        public static decimal Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, decimal>> selector)
             => source.Queryable.Average(selector);

        public static double? Average(this IRepositoryQuery<double?> source)
            => source.Queryable.Average();

        public static float? Average(this IRepositoryQuery<float?> source)
            => source.Queryable.Average();

        public static double? Average(this IRepositoryQuery<long?> source)
            => source.Queryable.Average();

        public static double? Average(this IRepositoryQuery<int?> source)
            => source.Queryable.Average();

        public static float Average<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, float>> selector)
            => source.Queryable.Average(selector);

        public static decimal? Average(this IRepositoryQuery<decimal?> source)
            => source.Queryable.Average();

        public static double Average(this IRepositoryQuery<long> source)
            => source.Queryable.Average();

        public static double Average(this IRepositoryQuery<int> source)
            => source.Queryable.Average();

        public static double Average(this IRepositoryQuery<double> source)
            => source.Queryable.Average();

        public static decimal Average(this IRepositoryQuery<decimal> source)
            => source.Queryable.Average();

        public static float Average(this IRepositoryQuery<float> source)
            => source.Queryable.Average();

        public static int Count<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.Count();

        public static int Count<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
            => source.Queryable.Count(predicate);

        public static TSource First<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.First();

        public static TSource First<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
            => source.Queryable.First(predicate);

        public static TSource? FirstOrDefault<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.FirstOrDefault();

        public static TSource? FirstOrDefault<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
            => source.Queryable.FirstOrDefault(predicate);

        public static long LongCount<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.LongCount();
        public static long LongCount<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
            => source.Queryable.LongCount(predicate);

        public static TSource Max<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.Max();

        public static TResult Max<TSource, TResult>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TResult>> selector)
            => source.Queryable.Max(selector);

        public static TSource Min<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.Min();

        public static TResult Min<TSource, TResult>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TResult>> selector)
            => source.Queryable.Min(selector);

        public static TSource Single<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.Single();

        public static TSource Single<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
            => source.Queryable.Single(predicate);

        public static TSource? SingleOrDefault<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.SingleOrDefault();

        public static TSource? SingleOrDefault<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
            => source.Queryable.SingleOrDefault(predicate);
        public static long Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, long>> selector)
            => source.Queryable.Sum(selector);
        public static decimal? Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, decimal?>> selector)
            => source.Queryable.Sum(selector);
        public static double? Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, double?>> selector)
            => source.Queryable.Sum(selector);
        public static int? Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, int?>> selector)
            => source.Queryable.Sum(selector);
        public static float? Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, float?>> selector)
            => source.Queryable.Sum(selector);
        public static float Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, float>> selector)
            => source.Queryable.Sum(selector);
        public static int Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, int>> selector)
            => source.Queryable.Sum(selector);
        public static long? Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, long?>> selector)
            => source.Queryable.Sum(selector);
        public static double Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, double>> selector)
            => source.Queryable.Sum(selector);
        public static decimal Sum<TSource>(this IRepositoryQuery<decimal> source)
            => source.Queryable.Sum();
        public static decimal? Sum<TSource>(this IRepositoryQuery<decimal?> source)
            => source.Queryable.Sum();
        public static float Sum<TSource>(this IRepositoryQuery<float> source)
            => source.Queryable.Sum();
        public static float? Sum<TSource>(this IRepositoryQuery<float?> source)
            => source.Queryable.Sum();
        public static long? Sum<TSource>(this IRepositoryQuery<long?> source)
            => source.Queryable.Sum();
        public static int? Sum<TSource>(this IRepositoryQuery<int?> source)
            => source.Queryable.Sum();
        public static double Sum<TSource>(this IRepositoryQuery<double> source)
            => source.Queryable.Sum();
        public static double? Sum<TSource>(this IRepositoryQuery<double?> source)
            => source.Queryable.Sum();
        public static long Sum<TSource>(this IRepositoryQuery<long> source)
            => source.Queryable.Sum();
        public static int Sum<TSource>(this IRepositoryQuery<int> source)
            => source.Queryable.Sum();
        public static decimal Sum<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, decimal>> selector)
            => source.Queryable.Sum(selector);

        public static TSource[] ToArray<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.ToArray();

        public static List<TSource> ToList<TSource>(this IRepositoryQuery<TSource> source)
            => source.Queryable.ToList();

        /// <summary>
        /// 提供页码和分页大小，从 <see cref="IRepositoryQuery{TSource}"/>  创建一个 <see cref="PageData{TSource}"/>。结果的 <see cref="PageData.Total"/> 属性返回的分页序列结果前将会自动计算序列总元素数。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要从其创建 <see cref="PageData{TSource}"/> 的 <see cref="IRepositoryQuery{TSource}"/>。</param>
        /// <param name="pagination">分页参数。</param>
        /// <returns>一个包含输入序列中元素的 <see cref="PageData{TSource}"/>。</returns>
        public static PageData<TSource> ToPage<TSource>(this IRepositoryQuery<TSource> source, IPagination pagination) => source.ToPage(pagination.Page, pagination.Limit);
    }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
