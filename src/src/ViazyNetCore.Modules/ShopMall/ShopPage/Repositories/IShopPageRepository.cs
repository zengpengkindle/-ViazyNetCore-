using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall.Repositories
{
    [Injection]
    public interface IShopPageRepository : IBaseRepository<ShopPage, long>
    {
        Task<PageData<ShopPage>> GetPageList(ShopPageQuery query);
    }
}
