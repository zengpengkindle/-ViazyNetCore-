using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.Manage.WebApi.Controllers
{
    [Route("api/shopmall/[action]")]
    public class PageController : BaseController
    {
        private readonly ShopPageService _shopPageService;

        public PageController(ShopPageService shopPageService)
        {
            this._shopPageService = shopPageService;
        }

        [HttpPost]
        public Task<PageData<ShopPage>> GetPageList(ShopPageQuery shopPageQuery)
        {
            return this._shopPageService.GetPageList(shopPageQuery);
        }

        [HttpPost]
        public async Task UpdatePage(ShopPageEditModel model)
        {
            if (model.Id == 0)
            {
                await this._shopPageService.AddPage(model);
            }
            else
            {
                await this._shopPageService.EditPage(model);
            }
        }

        [HttpPost]
        public async Task DeletePage(long id)
        {
            if (id == 0)
            {
                throw new ApiException("无效请求");
            }
            await this._shopPageService.DeletePage(id);
        }

        [HttpPost]
        public async Task SavePageDesign(PageDesignEditRes editRes)
        {
            await this._shopPageService.UpdateDesginAsync(editRes.Code, editRes.Items);
        }

        [HttpPost]
        public async Task<PageDesignEditRes> GetPageData(long id)
        {
            if (id == 0)
                throw new ApiException("无效请求");
            var page = await this._shopPageService.GetPageById(id);
            if (page == null)
                throw new ApiException("无效请求");
            var pageItems = await this._shopPageService.GetPageItems(page.Code);

            var result = new PageDesignEditRes
            {
                Code = page.Code,
                Items = pageItems
            };

            return result;
        }
    }
}
