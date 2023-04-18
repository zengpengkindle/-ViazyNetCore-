using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Model.ShopMall;

namespace ViazyNetCore.Modules.ShopMall.Repository
{
    [Injection]
    public class MemberFavProductRepository : DefaultRepository<MemberFavProduct, long>, IMemberFavProductRepository
    {
        public MemberFavProductRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public async Task<(long total, List<Product>)> FindAllAsync(Pagination args)
        {
            var result = await this.Select
                .From<Product>().InnerJoin((f, p) => f.ProductId == p.Id)
                .OrderByDescending((f, p) => f.CreateTime).WithTempQuery((f, p) => p).ToPageAsync(args);
            return (result.Total, result.Rows);
        }
    }
}
