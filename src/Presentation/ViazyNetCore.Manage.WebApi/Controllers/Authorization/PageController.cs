using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Domain;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Manage.WebApi.Controllers.Authorization
{
    /// <summary>
    /// 页面管理
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;
        public PageController(IPageService pageService)
        {
            this._pageService = pageService;
        }

        [ApiTitle("所有查询")]
        [Route("findAll"), HttpPost]
        public Task<PageData<PageFindAllModel>> FindAllAsync([Required] PageFindAllArgs args)
        {
            return this._pageService.FindAllAsync(args);
        }

        [ApiTitle("管理")]
        [Route("manage"), HttpPost]
        public Task<long> ManageAsync([Required] PageModel model)
        {
            //var roleId = this.HttpContext.GetAuthUser().AuthUserType;
            return this._pageService.ManageAsync(model, 0);
        }

        [ApiTitle("删除")]
        [Route("remove"), HttpPost]
        public Task RemoveAsync([Required] long id)
        {
            return this._pageService.RemoveAsync(id);
        }
    }
}
