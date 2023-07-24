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


        public static async Task<PageData<T2>> ToPageAsync<T1,T2>(this ISelect<T1> select, int? page, int? limit)
        {
            var pagingInfo = new BasePagingInfo
            {
                PageNumber = page ?? 1,
                PageSize = limit ?? 10
            };
            var items = await select.Page(pagingInfo).ToListAsync<T2>();
            return new PageData<T2>(items, pagingInfo.Count);
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
    }
}
