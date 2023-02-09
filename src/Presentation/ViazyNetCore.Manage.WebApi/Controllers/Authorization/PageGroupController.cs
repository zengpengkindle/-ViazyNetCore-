using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Domain;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Manage.WebApi.Controllers.Authorization
{
    /// <summary>
    /// 页面组管理
    /// </summary>
    [ApiController]
    [Route("pageGroup")]
    public class PageGroupController: ControllerBase
    {
        private readonly IPageGroupService _pageGroupService;

        public PageGroupController(IPageGroupService pageGroupService)
        {
            this._pageGroupService = pageGroupService;
        }

        /// <summary>
        /// 所有查询
        /// </summary>
        /// <returns></returns>
        [ApiTitle("所有查询")]
        [Route("findAll"), HttpPost]
        public Task<List<PageGroupModel>> FindAllAsync()
        {
            return this._pageGroupService.FindAllAsync(null);
        }

        /// <summary>
        /// 管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ApiTitle("管理")]
        [Route("manage"), HttpPost]
        public Task<long> ManageAsync([Required] PageGroupModel model)
        {
            return this._pageGroupService.ManageAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiTitle("删除")]
        [Route("remove"), HttpPost]
        public Task RemoveAsync([Required] long id)
        {
            return this._pageGroupService.RemoveAsync(id);
        }
    }
}
