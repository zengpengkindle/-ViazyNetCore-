using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall.Repositories
{
    [Injection]
    public class ShopPageItemRepository : DefaultRepository<ShopPageItem, long>, IShopPageItemRepository
    {
        public ShopPageItemRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
