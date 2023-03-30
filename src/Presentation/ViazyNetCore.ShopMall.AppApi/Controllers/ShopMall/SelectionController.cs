using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Model;

namespace ViazyNetCore.ShopMall.AppApi.Controllers
{
    /// <summary>
    /// 表示一个选品市场
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SelectionController : BaseController
    {
        private readonly ProductService _productService;

        public SelectionController(ProductService productService)
        {
            this._productService = productService;
        }

        [HttpPost]
        public async Task<PageData<Product>> Feed()
        {
            var query = new FindAllArguments()
            {
            };
            return await this._productService.FindProductAll(query);
        }
    }
}
