using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall.Repository
{
    [Injection]
    public class ProductCatRepository : DefaultRepository<ProductCat, string>, IProductCatRepository
    {
        public ProductCatRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public async Task<(long total, List<ProductCat>)> FindAllAsync(Pagination args)
        {
            var result = await this.Select.OrderByDescending(p => p.CreateTime).ToPageAsync(args);
            return (result.Total, result.Rows);
        }
    }
}
