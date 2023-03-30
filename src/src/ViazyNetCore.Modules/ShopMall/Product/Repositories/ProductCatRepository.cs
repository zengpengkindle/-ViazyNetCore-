using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall.Repositories
{
    [Injection]
    public class ProductCatRepository : DefaultRepository<ProductCat, string>, IProductCatRepository
    {
        public ProductCatRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
