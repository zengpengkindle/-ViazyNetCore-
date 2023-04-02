using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Modules.ShopMall.Models;

namespace ViazyNetCore.Manage.WebApi.Controllers.ShopMall
{
    /// <summary>
    /// 商品分类接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Area("shopmall")]
    [Permission(PermissionIds.Product)]
    public class ProductCatController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ProductCatService _productCatService;

        public ProductCatController(IMapper mapper, ProductCatService productCatService)
        {
            this._mapper = mapper;
            this._productCatService = productCatService;
        }

        /// <summary>
        /// 新增或编辑商品分类
        /// </summary>
        /// <param name="editReq"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Edit(CatEditReq editReq)
        {
            if (editReq.Id.IsNull())
            {
                var addDto = this._mapper.Map<CatEditReq, ProductCatAddDto>(editReq);
                await this._productCatService.AddCat(addDto);
            }
            else
            {
                var updateDto = this._mapper.Map<CatEditReq, ProductCatUpdateDto>(editReq);
                await this._productCatService.UpdateCat(updateDto);
            }
        }

        /// <summary>
        /// 搜索分类分页查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageData<CatRes>> FindPageList([FromQuery] PaginationSort pagination)
        {
            var result = await this._productCatService.FindAll(pagination);
            return new PageData<CatRes>
            {
                Total = result.Total,
                Rows = this._mapper.Map<List<ProductCatDto>, List<CatRes>>(result.Rows)
            };
        }

        [HttpPost]
        public async Task<List<CatRes>> GetAllList()
        {
            var result = await this._productCatService.GetProductCats(null);
            return this._mapper.Map<List<ProductCatDto>, List<CatRes>>(result);
        }

        [HttpPost]
        public async Task<CatRes> Get(string id)
        {
            var result = await this._productCatService.Get(id);
            return this._mapper.Map<ProductCatDto, CatRes>(result);
        }
    }
}
