using System.Providers;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore;

namespace ViazyNetCore.ShopMall.AppApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductCartController : BaseController
    {
        private readonly CartService _cartService;
        private readonly ILockProvider _lockProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _memberId => this._httpContextAccessor.HttpContext!.User.GetUserId();

        public ProductCartController(CartService cartService, ILockProvider lockProvider, IHttpContextAccessor httpContextAccessor)
        {
            this._cartService = cartService;
            this._lockProvider = lockProvider;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<ShoppingCart> FindCart()
        {
            var result = await this._cartService.GetShoppingCart(_memberId);

            for (int i = 0; i < result.Packages.Count; i++)
            {
                for (int j = 0; j < result.Packages[i].Items.Count; j++)
                {
                    result.Packages[i].Items[j].ImgUrl = result.Packages[i].Items[j].ImgUrl.ToCdnUrl();
                    result.Packages[i].Items[j].Num = 1;
                }
            }
            return result;
        }

        [HttpPost]
        public async Task<bool> AddCart(ShoppingCartEditDto product)
        {
            var result = await this._cartService.AddShoppingCartProduct(product, _memberId);
            if (!result)
                throw new ApiException("购物车商品添加失败");
            return true;
        }

        [HttpPost]
        public async Task<bool> RemoveCart(ShoppingCartEditDto product)
        {
            var result = await this._cartService.RemoveShoppingCartProduct(product, _memberId);
            if (!result)
                throw new ApiException("购物车商品移除失败");
            return true;
        }
    }

    public class CartItemModel
    {
        public string GoodsId { get; set; }

        public int SelectedNum { get; set; }
    }

    public class CartItemSkuModel
    {
        public string Id { get; set; }

        public decimal Price { get; set; }
    }
}
