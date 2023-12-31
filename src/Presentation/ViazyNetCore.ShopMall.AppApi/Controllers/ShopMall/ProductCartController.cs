﻿using System.Providers;
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

        public ProductCartController(CartService cartService, ILockProvider lockProvider, IHttpContextAccessor httpContextAccessor)
        {
            this._cartService = cartService;
            this._lockProvider = lockProvider;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<ShoppingCart> FindCart()
        {
            var result = await this._cartService.GetShoppingCart(this.MemberId);

            foreach (var package in result.Packages)
            {
                foreach (var item in package.Items)
                {
                    item.ImgUrl = item.ImgUrl.ToCdnUrl();
                    item.Num = item.Num == 0 ? 1 : item.Num;
                }
            }
            return result;
        }

        [HttpPost]
        public async Task<bool> AddCart(ShoppingCartEditDto product)
        {
            var result = await this._cartService.AddShoppingCartProduct(product, this.MemberId);
            if (!result)
                throw new ApiException("购物车商品添加失败");
            return true;
        }

        [HttpPost]
        public async Task<bool> RemoveCart(ShoppingCartEditDto product)
        {
            var result = await this._cartService.RemoveShoppingCartProduct(product, this.MemberId);
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
