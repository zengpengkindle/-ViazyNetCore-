using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall.Repositories
{
    [Injection]
    public class ShopPageRepository : DefaultRepository<ShopPage, long>, IShopPageRepository
    {
        public ShopPageRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public Task<ShopPage> GetByCode(string code)
        {
            return this.Select.Where(p => p.Code == code).ToOneAsync();
        }

        public Task<PageData<ShopPage>> GetPageList(ShopPageQuery query)
        {
            return this.Select.WhereIf(query.Layout.HasValue, p => p.Layout == query.Layout)
                  .WhereIf(query.Type.HasValue, p => p.Type == query.Type)
                  .WhereIf(query.Code.IsNotNull(), p => p.Code == query.Code)
                  .WhereIf(query.Name.IsNotNull(), p => p.Name.Contains(query.Name))
                  .ToPageAsync(query);
        }
    }
}
