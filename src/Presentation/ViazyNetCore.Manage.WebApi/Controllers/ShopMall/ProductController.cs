using System.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.Manage.WebApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Area("shopmall")]
    [Permission(PermissionIds.Product)]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ILockProvider _lockProvider;
        private readonly string _shopId = "10000";//假定登陆的商家编号

        public ProductController(ProductService productService, ILockProvider lockProvider)
        {
            this._productService = productService;
            this._lockProvider = lockProvider;
        }
        [HttpPost]
        [Permission(PermissionIds.Stock, PermissionIds.Product)]
        public async Task<PageData<Product>> FindAll(FindAllArguments args)
        {
            args.ShopId = _shopId;
            return await this._productService.FindProductAll(args);
        }

        [HttpPost]
        [Permission(PermissionIds.Stock, PermissionIds.Product)]
        public async Task<ProductModifyModel> Find(string id, string? outerType)
        {
            var product = new ProductManageModel();
            if (id.IsNotNull())
            {
                var value = await this._productService.GetManageProductInfo(id, outerType);
                product = value ?? throw new ApiException("未找到商品");
            }

            //var brands = await this._productService.GetProductBrands();
            //var cats = await this._productService.GetProductCats();

            return new ProductModifyModel { Product = product };
        }

        [HttpPost]
        [Permission(PermissionIds.Stock, PermissionIds.Product)]
        public Task<List<ProductBrand>> GetBrands()
        {
            return this._productService.GetProductBrands();
        }

        [HttpPost]
        [Permission(PermissionIds.Stock, PermissionIds.Product)]
        public Task<List<ProductCatDto>> GetCats([FromServices] ProductCatService productCatService)
        {
            return productCatService.GetProductCats();
        }

        [HttpPost]
        public async Task<bool> ModifyStatus(string id, ProductStatus status)
        {
            await this._productService.ModifyProductStatus(id, this._shopId, status);
            return true;
        }

        [HttpPost]
        public async Task<bool> Remove(string id)
        {
            await this._productService.ModifyProductStatus(id, this._shopId, ProductStatus.Delete);
            return true;
        }

        [HttpPost]
        public async Task Submit(string? outerType, ProductManageModel item)
        {

            item.ShopId = this._shopId;
            item.ShopName = "测试";
            //item.Image = "";
            if (item.Image.IsNull())
                throw new ApiException("请上传商品主图");
            await this._productService.ManageProduct(item, outerType);
        }
    }

    public class ProductModifyModel
    {
        public ProductManageModel Product { get; set; }

        public List<ProductBrand> Brands { get; set; }

        public List<ProductCat> Cats { get; set; }
    }
}
