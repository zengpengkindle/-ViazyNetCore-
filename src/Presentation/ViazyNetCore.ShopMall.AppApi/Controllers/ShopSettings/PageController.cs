using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ViazyNetCore.Model;

namespace ViazyNetCore.ShopMall.AppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PageController : BaseController
    {
        private readonly ShopPageService _shopPageService;

        public PageController(ShopPageService shopPageService)
        {
            this._shopPageService = shopPageService;
        }

        [HttpPost]
        public async Task<List<DesginItem>> GetPageData(string code
            , [FromServices] ProductService productService)
        {
            var pageItems = await this._shopPageService.GetPageItems(code);

            foreach (var pageItem in pageItems)
            {
                if (pageItem.Type == "goods")
                {
                    var item = pageItem.Value.ToObject<GoodsPageItem>();
                    if (item != null)
                    {
                        var products = await productService.FindProductAll(new FindAllArguments
                        {
                            ShopId = "10000",
                            CatId = item.ClassifyId,
                            Page = 1,
                            Limit = item.Limit == 0 ? 10 : item.Limit
                        });
                        var items = products.Rows.Select(p => new GoodsPageProductItem
                        {
                            BrandId = p.BrandId,
                            BrandName = p.BrandName,
                            CatId = p.CatId,
                            CatName = p.CatName,
                            Description = p.Description,
                            Id = p.Id,
                            Image = p.Image.ToCdnUrl(),
                            Keywords = p.Keywords,
                            OpenSpec = p.OpenSpec,
                            Price = p.Price,
                            SubTitle = p.SubTitle,
                            Name = p.Title,
                        }).ToList();
                        item.List = items;
                        pageItem.Value = JObject.FromObject(item, JsonSerializer.Create(JSON.SerializerSettings));
                    }
                }
            }
            return pageItems;
        }
    }
}
