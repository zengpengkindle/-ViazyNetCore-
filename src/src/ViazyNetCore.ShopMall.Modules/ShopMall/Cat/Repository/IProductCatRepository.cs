using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall.Repository
{
    [Injection]
    public interface IProductCatRepository : IBaseRepository<ProductCat, string>
    {
        Task<(long total, List<ProductCat>)> FindAllAsync(Pagination args);
    }
}
