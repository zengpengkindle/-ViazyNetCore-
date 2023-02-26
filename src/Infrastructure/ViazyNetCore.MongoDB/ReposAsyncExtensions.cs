using System.Collections.Generic;
using System.Linq.Expressions;
using System.Repos;
using System.Threading.Tasks;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace System.Linq
{
#pragma warning disable CS1591,CS8619,CS8604 // 缺少对公共可见类型或成员的 XML 注释
    public static class ReposAsyncExtensions
    {
        public static Task<bool> AllAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
                    => Task.FromResult((source.Queryable as IMongoQueryable<TSource>).All(predicate));

        public static Task<bool> AnyAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => (source.Queryable as IMongoQueryable<TSource>).AnyAsync();

        public static Task<bool> AnyAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
                    => (source.Queryable as IMongoQueryable<TSource>).AnyAsync(predicate);

        public static Task<double> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, double>> selector)
                     => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<float?> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, float?>> selector)
                     => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<double?> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, long?>> selector)
                     => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<double?> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, int?>> selector)
                     => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<double?> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, double?>> selector)
                     => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<decimal?> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, decimal?>> selector)
                     => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<double> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, long>> selector)
                     => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<double> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, int>> selector)
                     => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<decimal> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, decimal>> selector)
                     => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<double?> Average(this IRepositoryQuery<double?> source)
                    => (source.Queryable as IMongoQueryable<double?>).AverageAsync();

        public static Task<float?> Average(this IRepositoryQuery<float?> source)
                    => (source.Queryable as IMongoQueryable<float?>).AverageAsync();

        public static Task<double?> Average(this IRepositoryQuery<long?> source)
                    => (source.Queryable as IMongoQueryable<long?>).AverageAsync();

        public static Task<double?> Average(this IRepositoryQuery<int?> source)
                    => (source.Queryable as IMongoQueryable<int?>).AverageAsync();

        public static Task<float> AverageAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, float>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).AverageAsync(selector);

        public static Task<decimal?> Average(this IRepositoryQuery<decimal?> source)
                    => (source.Queryable as IMongoQueryable<decimal?>).AverageAsync();

        public static Task<double> Average(this IRepositoryQuery<long> source)
                    => (source.Queryable as IMongoQueryable<long>).AverageAsync();

        public static Task<double> Average(this IRepositoryQuery<int> source)
                    => (source.Queryable as IMongoQueryable<int>).AverageAsync();

        public static Task<double> Average(this IRepositoryQuery<double> source)
                    => (source.Queryable as IMongoQueryable<double>).AverageAsync();

        public static Task<decimal> Average(this IRepositoryQuery<decimal> source)
                    => (source.Queryable as IMongoQueryable<decimal>).AverageAsync();

        public static Task<float> Average(this IRepositoryQuery<float> source)
                    => (source.Queryable as IMongoQueryable<float>).AverageAsync();

        public static Task<int> CountAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => (source.Queryable as IMongoQueryable<TSource>).CountAsync();

        public static Task<int> CountAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
                    => (source.Queryable as IMongoQueryable<TSource>).CountAsync(predicate);

        public static Task<TSource> FirstAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => (source.Queryable as IMongoQueryable<TSource>).FirstAsync();

        public static Task<TSource> FirstAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
                    => (source.Queryable as IMongoQueryable<TSource>).FirstAsync(predicate);

        public static Task<TSource?> FirstOrDefaultAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => (source.Queryable as IMongoQueryable<TSource>).FirstOrDefaultAsync();

        public static Task<TSource?> FirstOrDefaultAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
                    => (source.Queryable as IMongoQueryable<TSource>).FirstOrDefaultAsync(predicate);

        public static Task<long> LongCountAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => (source.Queryable as IMongoQueryable<TSource>).LongCountAsync();
        public static Task<long> LongCountAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
                    => (source.Queryable as IMongoQueryable<TSource>).LongCountAsync(predicate);

        public static Task<TSource> MaxAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => (source.Queryable as IMongoQueryable<TSource>).MaxAsync();

        public static Task<TResult> MaxAsync<TSource, TResult>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TResult>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).MaxAsync(selector);

        public static Task<TSource> MinAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => (source.Queryable as IMongoQueryable<TSource>).MinAsync();

        public static Task<TResult> MinAsync<TSource, TResult>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, TResult>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).MinAsync(selector);

        public static Task<TSource> SingleAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => (source.Queryable as IMongoQueryable<TSource>).SingleAsync();

        public static Task<TSource> SingleAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
                    => (source.Queryable as IMongoQueryable<TSource>).SingleAsync(predicate);

        public static Task<TSource?> SingleOrDefaultAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => (source.Queryable as IMongoQueryable<TSource>).SingleOrDefaultAsync();

        public static Task<TSource?> SingleOrDefaultAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, bool>> predicate)
                    => (source.Queryable as IMongoQueryable<TSource>).SingleOrDefaultAsync(predicate);
        public static Task<long> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, long>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        public static Task<decimal?> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, decimal?>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        public static Task<double?> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, double?>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        public static Task<int?> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, int?>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        public static Task<float?> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, float?>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        public static Task<float> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, float>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        public static Task<int> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, int>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        public static Task<long?> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, long?>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        public static Task<double> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, double>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        public static Task<decimal> SumAsync<TSource>(this IRepositoryQuery<decimal> source)
                    => (source.Queryable as IMongoQueryable<decimal>).SumAsync();
        public static Task<decimal?> SumAsync<TSource>(this IRepositoryQuery<decimal?> source)
                    => (source.Queryable as IMongoQueryable<decimal?>).SumAsync();
        public static Task<float> SumAsync<TSource>(this IRepositoryQuery<float> source)
                    => (source.Queryable as IMongoQueryable<float>).SumAsync();
        public static Task<float?> SumAsync<TSource>(this IRepositoryQuery<float?> source)
                    => (source.Queryable as IMongoQueryable<float?>).SumAsync();
        public static Task<long?> SumAsync<TSource>(this IRepositoryQuery<long?> source)
                    => (source.Queryable as IMongoQueryable<long?>).SumAsync();
        public static Task<int?> SumAsync<TSource>(this IRepositoryQuery<int?> source)
                    => (source.Queryable as IMongoQueryable<int?>).SumAsync();
        public static Task<double> SumAsync<TSource>(this IRepositoryQuery<double> source)
                    => (source.Queryable as IMongoQueryable<double>).SumAsync();
        public static Task<double?> SumAsync<TSource>(this IRepositoryQuery<double?> source)
                    => (source.Queryable as IMongoQueryable<double?>).SumAsync();
        public static Task<long> SumAsync<TSource>(this IRepositoryQuery<long> source)
                    => (source.Queryable as IMongoQueryable<long>).SumAsync();
        public static Task<int> SumAsync<TSource>(this IRepositoryQuery<int> source)
                    => (source.Queryable as IMongoQueryable<int>).SumAsync();
        public static Task<decimal> SumAsync<TSource>(this IRepositoryQuery<TSource> source, Expression<Func<TSource, decimal>> selector)
                    => (source.Queryable as IMongoQueryable<TSource>).SumAsync(selector);
        private static async Task<List<TSource>> OnToListAsync<TSource>(this IRepositoryQuery<TSource> source)
        {
            using var currsor = await source.Queryable.MustBe<IMongoQueryable<TSource>>().ToCursorAsync().ConfigureAwait(false);

            var list = new List<TSource>();
            while(await currsor.MoveNextAsync().ConfigureAwait(false))
            {
                list.AddRange(currsor.Current);
            }
            list.TrimExcess();
            return list;

        }
        public static Task<TSource[]> ToArrayAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => source.OnToListAsync().ContinueWith(t => t.Result.ToArray());

        public static Task<List<TSource>> ToListAsync<TSource>(this IRepositoryQuery<TSource> source)
                    => source.OnToListAsync();

        /// <summary>
        /// 提供页码和分页大小，从 <see cref="IRepositoryQuery{TSource}"/>  创建一个 <see cref="PageData{TSource}"/>。结果的 <see cref="PageData.Total"/> 属性返回的分页序列结果前将会自动计算序列总元素数。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要从其创建 <see cref="PageData{TSource}"/> 的 <see cref="IRepositoryQuery{TSource}"/>。</param>
        /// <param name="pagination">分页参数。</param>
        /// <returns>一个包含输入序列中元素的 <see cref="PageData{TSource}"/>。</returns>
        public static Task<PageData<TSource>> ToPageAsync<TSource>(this IRepositoryQuery<TSource> source, IPagination pagination) => source.ToPageAsync(pagination.PageNumber, pagination.PageSize);
    }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}

