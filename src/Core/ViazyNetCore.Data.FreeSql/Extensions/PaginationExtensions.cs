using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.Internal.Model;
using FreeSql;

namespace ViazyNetCore
{
    public static class PaginationExtensions
    {
        public static async Task<PageData<T1>> ToPageAsync<T1>(this ISelect<T1> select, Pagination args)
        {
            var items = await select.ToPageAsync(args.Page, args.Limit);
            return items;
        }

        public static async Task<PageData<T1>> ToPageAsync<T1>(this ISelect<T1> select, int? page, int? limit)
        {
            var pagingInfo = new BasePagingInfo
            {
                PageNumber = page ?? 1,
                PageSize = limit ?? 10
            };
            var items = await select.Page(pagingInfo).ToListAsync();
            return new PageData<T1>(items, pagingInfo.Count);
        }

        public static async Task<PageData<TReturn>> ToPageAsync<T1, TReturn>(this ISelect<T1> select, int? page, int? limit)
        {
            var pagingInfo = new BasePagingInfo
            {
                PageNumber = page ?? 1,
                PageSize = limit ?? 10
            };
            var items = await select.Page(pagingInfo).ToListAsync<TReturn>();
            return new PageData<TReturn>(items, pagingInfo.Count);
        }

        public static async Task<PageData<TReturn>> ToPageAsync<T1, T2, TReturn>(this ISelect<T1, T2> select, int? page, int? limit)
            where T2 : class
        {
            var pagingInfo = new BasePagingInfo
            {
                PageNumber = page ?? 1,
                PageSize = limit ?? 10
            };
            var items = await select.Page(pagingInfo).ToListAsync<TReturn>();
            return new PageData<TReturn>(items, pagingInfo.Count);
        }

        public static PageData<T1> ToPage<T1>(this ISelect<T1> select, int page, int limit)
        {
            var pagingInfo = new BasePagingInfo
            {
                PageNumber = page,
                PageSize = limit
            };
            var items = select.Page(pagingInfo).ToList();
            return new PageData<T1>(items, pagingInfo.Count);
        }

        public static FreeSql.ISelect<T> PageSort<T>(this ISelect<T> fsql, out BasePagingInfo page, Func<string, int, ISelect<T>, FreeSql.ISelect<T>> sortAction, PaginationSort pagePagination, bool returnTotal = true)
        {
            page = new BasePagingInfo
            {
                PageNumber = pagePagination?.Page ?? 1,
                PageSize = pagePagination?.Limit ?? 10
            };
            var sort = pagePagination.Sort;
            var sortField = pagePagination.SortField;
            fsql = sortAction.Invoke(sortField, sort, fsql);

            if (returnTotal)
            {
                fsql = fsql.Page(page);
            }
            else
            {
                fsql = fsql.Page(page.PageNumber, page.PageSize);
            }

            return fsql;
        }

        public static FreeSql.ISelect<T1, T2> PageSort<T1, T2>(this ISelect<T1, T2> fsql, out BasePagingInfo page, Func<string, int, ISelect<T1, T2>, FreeSql.ISelect<T1, T2>> sortAction, PaginationSort pagePagination, bool returnTotal = true)
            where T2 : class
        {
            page = new BasePagingInfo
            {
                PageNumber = pagePagination?.Page ?? 1,
                PageSize = pagePagination?.Limit ?? 10
            };
            var sort = pagePagination.Sort;
            var sortField = pagePagination.SortField;

            fsql = sortAction.Invoke(sortField, sort, fsql);

            if (returnTotal)
            {
                fsql = fsql.Page(page);
            }
            else
            {
                fsql = fsql.Page(page.PageNumber, page.PageSize);
            }

            return fsql;
        }

        public static FreeSql.ISelect<T1, T2, T3> PageSort<T1, T2, T3>(this ISelect<T1, T2, T3> fsql, out BasePagingInfo page, Func<string, int, ISelect<T1, T2, T3>, FreeSql.ISelect<T1, T2, T3>> sortAction, PaginationSort pagePagination, bool returnTotal = true)
            where T2 : class where T3 : class
        {
            page = new BasePagingInfo
            {
                PageNumber = pagePagination?.Page ?? 1,
                PageSize = pagePagination?.Limit ?? 10
            };
            var sort = pagePagination.Sort;
            var sortField = pagePagination.SortField;

            fsql = sortAction.Invoke(sortField, sort, fsql);

            if (returnTotal)
            {
                fsql = fsql.Page(page);
            }
            else
            {
                fsql = fsql.Page(page.PageNumber, page.PageSize);
            }

            return fsql;
        }

        public static FreeSql.ISelect<T1, T2, T3, T4> PageSort<T1, T2, T3, T4>(this ISelect<T1, T2, T3, T4> fsql, out BasePagingInfo page, Func<string, int, ISelect<T1, T2, T3, T4>, FreeSql.ISelect<T1, T2, T3, T4>> sortAction, PaginationSort pagePagination, bool returnTotal = true)
            where T2 : class where T3 : class where T4 : class
        {
            page = new BasePagingInfo
            {
                PageNumber = pagePagination?.Page ?? 1,
                PageSize = pagePagination?.Limit ?? 10
            };
            var sort = pagePagination.Sort;
            var sortField = pagePagination.SortField;

            fsql = sortAction.Invoke(sortField, sort, fsql);

            if (returnTotal)
            {
                fsql = fsql.Page(page);
            }
            else
            {
                fsql = fsql.Page(page.PageNumber, page.PageSize);
            }

            return fsql;
        }

        public static FreeSql.ISelect<T1, T2, T3, T4, T5> PageSort<T1, T2, T3, T4, T5>(this ISelect<T1, T2, T3, T4, T5> fsql, out BasePagingInfo page, Func<string, int, ISelect<T1, T2, T3, T4, T5>, FreeSql.ISelect<T1, T2, T3, T4, T5>> sortAction, PaginationSort pagePagination, bool returnTotal = true)
            where T2 : class where T3 : class where T4 : class where T5 : class
        {
            page = new BasePagingInfo
            {
                PageNumber = pagePagination?.Page ?? 1,
                PageSize = pagePagination?.Limit ?? 10
            };
            var sort = pagePagination.Sort;
            var sortField = pagePagination.SortField;

            fsql = sortAction.Invoke(sortField, sort, fsql);

            if (returnTotal)
            {
                fsql = fsql.Page(page);
            }
            else
            {
                fsql = fsql.Page(page.PageNumber, page.PageSize);
            }

            return fsql;
        }

    }
}
