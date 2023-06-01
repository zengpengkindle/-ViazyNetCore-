using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.ShopMall.Manage.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Area("shopmall")]
    public class ProductOuterController : ControllerBase
    {
        private readonly ProductOuterService _productOuterService;

        public ProductOuterController(ProductOuterService productOuterService)
        {
            this._productOuterService = productOuterService;
        }

        [HttpPost]
        public Task<PageData<ProductOuter>> FindAll(Pagination args)
        {
            return this._productOuterService.FindAll(args);
        }

        [HttpPost]
        public Task<ProductOuter> GetAsync(string id)
        {
            return this._productOuterService.GetAsync(id);
        }

        [HttpPost]
        public async Task<bool> MangerProductOuter(ProductOuter productOuter)
        {
            if (productOuter.Id.IsNull())
            {
                productOuter.Id = Snowflake<ProductOuter>.NextIdString();
                productOuter.Status = ComStatus.Enabled;
                productOuter.CreateTime = DateTime.Now;
                await this._productOuterService.AddProductOuter(productOuter);
            }
            else
            {
                await this._productOuterService.ManageProductOuter(productOuter);
            }
            return true;
        }

        [HttpPost]
        public async Task<bool> ModifyStatus(string outerId, ComStatus status)
        {
            await this._productOuterService.ModifyStatus(outerId, status);
            return true;
        }

        [HttpPost]
        public Task<Dictionary<string, string>> GetAllAsync()
        {
            return this._productOuterService.GetAllAsync();
        }
    }
}
