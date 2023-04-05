using System.ComponentModel.DataAnnotations;
using System.Providers;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.ShopMall.AppApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : BaseController
    {
        private readonly ProductService _productService;
        private readonly ILockProvider _lockProvider;
        private readonly string _shopId = "123456";
        private readonly string _imgBaseUrl = @"http://localhost:7277";

        public ProductController(ProductService productService, ILockProvider lockProvider)
        {
            this._productService = productService;
            this._lockProvider = lockProvider;
        }

        [HttpPost]
        public async Task<ProductInfoModel> GetProductSku([Required] string productId, string outerType)
        {
            var productSkus = await this._productService.FindProductInfo(productId);
            if (productSkus == null)
            {
                throw new ApiException("未知商品");
            }

            productSkus.Image = _imgBaseUrl + productSkus.Image;
            productSkus.SubImage = productSkus.SubImage.Replace("/upload/", _imgBaseUrl + "/upload/");
            productSkus.Detail = productSkus.Detail.Replace("/upload/", _imgBaseUrl + "/upload/");

            productSkus.Skus = await this._productService.GetProductSku(productId, outerType);

            if (productSkus.Skus.Tree.Count > 0)
            {
                for (int i = 0; i < productSkus.Skus.Tree[0].V.Count; i++)
                {
                    productSkus.Skus.Tree[0].V[i].ImgUrl = productSkus.Skus.Tree[0].V[i].ImgUrl.Replace("/upload/", _imgBaseUrl + "/upload/");
                }
            }
            //因为前端sku价格单位为分 需乘以100
            for (int i = 0; i < productSkus.Skus.List.Count; i++)
            {
                productSkus.Skus.List[i].Price = productSkus.Skus.List[i].Price * 100;
            }

            return productSkus;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取商品基本信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ProductInfoModel> GetProductInfo(string productId)
        {
            if (productId.IsNull())
                throw new ApiException("数据异常");

            var info = await this._productService.FindProductInfo(productId);
            info.Image = info.Image.ToCdnUrl();
            info.SubImage = info.SubImage.ToCdnUrl();
            info.Detail = info.Detail.ToCdnUrl();
            return info;
        }
    }

    public class SKUModel
    {

    }
}
