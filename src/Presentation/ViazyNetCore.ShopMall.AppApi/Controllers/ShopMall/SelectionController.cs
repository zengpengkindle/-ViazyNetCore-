using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Model;
using ViazyNetCore.Modules.ShopMall.Models;

namespace ViazyNetCore.ShopMall.AppApi.Controllers
{
    /// <summary>
    /// 表示一个选品市场
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SelectionController : BaseController
    {
        private readonly SelectionService _selectionService;

        public SelectionController(SelectionService selectionService)
        {
            this._selectionService = selectionService;
        }

        [HttpPost]
        public async Task<MorePageData<SelectionFeedListDto>> Feed([FromQuery] Pagination pagination, SelectionFeedQueryReq req)
        {
            var query = new FindAllArguments()
            {
                Limit = pagination.Limit,
                Page = pagination.Page,
                CatId = req.CatId
            };
            var (_, list) = await this._selectionService.FindProductAll(query);
            list.ForEach(item =>
            {
                item.Image = item.Image.ToCdnUrl();
            });
            return new MorePageData<SelectionFeedListDto>
            {
                HasMore = list.Count == pagination.Limit,
                Rows = list
            };
        }
    }
}
