using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Model;

namespace ViazyNetCore.ShopMall.AppApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductCatController : BaseController
    {
        private readonly ProductCatService _productCatService;

        public ProductCatController(ProductCatService productCatService)
        {
            this._productCatService = productCatService;
        }

        [HttpPost]
        public Task<List<ProductCat>> GetCats()
        {
            return this._productCatService.GetProductCats();
        }
    }
}
