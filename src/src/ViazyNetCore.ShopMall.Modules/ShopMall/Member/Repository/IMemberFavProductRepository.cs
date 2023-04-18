using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Model.ShopMall;

namespace ViazyNetCore.Modules.ShopMall.Repository
{
    [Injection]
    public interface IMemberFavProductRepository : IBaseRepository<MemberFavProduct, long>
    {
        Task<(long total, List<Product>)> FindAllAsync(Pagination args);
    }
}
