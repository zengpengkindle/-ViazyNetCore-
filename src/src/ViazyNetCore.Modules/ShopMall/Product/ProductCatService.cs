using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.ShopMall.Repositories;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class ProductCatService
    {
        private readonly IProductCatRepository _productCatRepository;

        public ProductCatService(IProductCatRepository productCatRepository)
        {
            this._productCatRepository = productCatRepository;
        }

        public Task<List<ProductCat>> GetProductCats()
        {
            return this._productCatRepository.Where(t => t.Status == ComStatus.Enabled).ToListAsync();
        }
    }
}
