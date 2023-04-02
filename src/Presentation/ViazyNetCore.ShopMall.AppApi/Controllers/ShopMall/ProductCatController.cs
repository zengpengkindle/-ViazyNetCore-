using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Model;
using ViazyNetCore.Modules.ShopMall.Models;

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
        public async Task<List<CatRes>> GetCats()
        {
            var list = await this._productCatService.GetProductCats();
            return list.Select(p => new CatRes
            {
                Id = p.Id,
                Image = p.Image.ToCdnUrl(),
                IsParent = p.ParentId.IsNull(),
                Name = p.Name,
                ParentId = p.ParentId,
                Path = p.Path,
                Sort = p.Sort,
            }).ToList();
        }
    }
}
