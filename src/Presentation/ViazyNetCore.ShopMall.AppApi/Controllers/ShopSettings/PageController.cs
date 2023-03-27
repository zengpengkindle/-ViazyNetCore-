using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<DesginItem>> GetPageData(string code)
        {
            return await this._shopPageService.GetPageItems(code);
        }
    }
}
