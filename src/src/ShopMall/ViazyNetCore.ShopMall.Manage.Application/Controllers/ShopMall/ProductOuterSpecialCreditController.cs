using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.ShopMall.Manage.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Area("shopmall")]
    public class ProductOuterSpecialCreditController : ControllerBase
    {
        private readonly ProductOuterSpecialCreditService _productOuterSpecialCreditService;

        public ProductOuterSpecialCreditController(ProductOuterSpecialCreditService productOuterSpecialCreditService)
        {
            this._productOuterSpecialCreditService = productOuterSpecialCreditService;
        }

        [HttpPost]
        public Task<PageData<ProductOuterSpecialCredit>> FindAll(SpecialCreditPagination args)
        {
            return this._productOuterSpecialCreditService.FindAll(args);
        }

        [HttpPost]
        public Task<ProductOuterSpecialCredit> GetAsync(string id)
        {
            return this._productOuterSpecialCreditService.GetAsync(id);
        }

        [HttpPost]
        public Task MangerAsync(ProductOuterSpecialCredit productOuter)
        {
            if (productOuter.Id.IsNull())
            {
                productOuter.Id = Snowflake<ProductOuterSpecialCredit>.NextIdString();
                productOuter.Status = ComStatus.Enabled;
                productOuter.CreateTime = DateTime.Now;
                return this._productOuterSpecialCreditService.AddAsync(productOuter);
            }
            else
            {
                return this._productOuterSpecialCreditService.ManageAsync(productOuter);
            }
        }

        [HttpPost]
        public Task ModifyStatus(string id, ComStatus status)
        {
            return this._productOuterSpecialCreditService.ModifyStatus(id, status);
        }

        [HttpPost]
        public Task<List<OuterKeySpecialCredit>> GetSpecialCreditByOuterKey(string outerType)
        {
            return this._productOuterSpecialCreditService.GetSpecialCreditByOuterKey(outerType);
        }
    }
}
