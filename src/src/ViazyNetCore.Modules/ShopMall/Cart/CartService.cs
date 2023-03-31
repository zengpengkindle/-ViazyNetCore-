using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class CartService
    {
        private readonly IFreeSql _engine;
        private readonly IEnumerable<IAddCartHanlder> _addCartHandlers;
        private readonly IEnumerable<IChangeCartHanlder> _changeCartHanlder;
        private readonly IEnumerable<IRemoveCartHanlder> _removeCartHanlder;
        private readonly IEnumerable<IGetCartHanlder> _getCartHanlder;
        public CartService(IFreeSql engine, ILogger<CartService> logger,
            IEnumerable<IAddCartHanlder> addCartHandlers,
            IEnumerable<IChangeCartHanlder> changeCartHanlde,
            IEnumerable<IRemoveCartHanlder> removeCartHanlder,
            IEnumerable<IGetCartHanlder> getCartHanlder)
        {
            this._engine = engine;
            this._addCartHandlers = addCartHandlers;
            this._changeCartHanlder = changeCartHanlde;
            this._removeCartHanlder = removeCartHanlder;
            this._getCartHanlder = getCartHanlder;
        }

        /// <summary>
        /// 获取用户购物车信息
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> GetShoppingCart(string memberId)
        {
            var cartItems = await this._engine.Select<MemberCarItem>().Where(t => t.CarId == memberId).ToListAsync();

            var cart = new ShoppingCart();
            cart.Num = cartItems.Count;

            foreach (var item in cartItems)
            {
                var value = new ShoppingCartProduct();

                value.Id = item.Id;

                value.ImgUrl = item.Image;
                value.PId = item.ProductId;
                value.Pn = item.Title;
                value.ShopId = item.ShopId;
                value.SkuId = item.SkuId;
                value.SkuText = item.SkuText;
                value.Status = ComStatus.Enabled;

                var product = await this._engine.Select<Product>().Where(t => t.Id == item.ProductId).FirstAsync();

                if (product == null) { value.Status = ComStatus.Deleted; value.Price = item.Price; }
                else
                    value.Price = product.Price;

                if (product.Status != ProductStatus.OnSale) value.Status = ComStatus.Disabled;

                if (item.SkuId.IsNull())
                {
                    var sku = await this._engine.Select<ProductSku>().Where(t => t.Id == item.SkuId && t.ProductId == item.ProductId).FirstAsync();
                    if (sku == null)
                    {
                        value.Status = ComStatus.Deleted;
                        value.Price = item.Price;
                    }
                    else
                        value.Price = sku.Price;

                }

                if (cart.Packages.Any(t => t.ShopId == item.ShopId))
                {
                    var pack = cart.Packages.Where(t => t.ShopId == item.ShopId).SingleOrDefault();
                    pack.Items.Add(value);

                }
                else
                {
                    var pack = new ShoppingCartPackage();
                    pack.ShopId = item.ShopId;
                    var shop = await this._engine.Select<Model.Shop>().Where(t => t.Id == item.ShopId).ToOneAsync();
                    if (shop != null)
                        pack.ShopName = shop.ComponyName;

                    pack.Items.Add(value);

                    cart.Packages.Add(pack);
                }
            }
            if (_getCartHanlder != null)
            {
                foreach (var handler in _getCartHanlder)
                {
                    await handler.OnFindCartAsync(memberId);
                }
            }

            return cart;
        }

        /// <summary>
        /// 添加购物车商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> AddShoppingCartProduct(ShoppingCartProduct carItem, string memberId)
        {
            var product = await this._engine.Select<Product>().Where(t => t.Id == carItem.PId).FirstAsync();
            if (product == null)
                return false;
            var query = this._engine.Select<MemberCarItem>().Where(t => t.ProductId == product.Id && t.CarId == memberId);

            if (carItem.SkuId.IsNotNull())
                query = query.Where(t => t.SkuId == carItem.SkuId);



            var item = await query.FirstAsync();
            if (item == null)
            {
                var newItem = new MemberCarItem();
                newItem.Id = Snowflake<MemberCarItem>.NextIdString();
                newItem.AddTime = DateTime.Now;
                newItem.CarId = memberId;
                newItem.ChangedTime = DateTime.Now;
                newItem.Num = carItem.Num;
                newItem.ProductId = carItem.PId;
                newItem.ShopId = product.ShopId;
                newItem.SkuId = carItem.SkuId;
                newItem.SkuText = carItem.SkuText;
                newItem.Title = product.Title;

                newItem.Freight = product.Freight;
                newItem.FreightStep = product.FreightStep;

                newItem.Image = product.Image;

                if (carItem.SkuId.IsNotNull())
                {
                    var sku = await this._engine.Select<ProductSku>().Where(t => t.Id == carItem.SkuId).FirstAsync();
                    if (sku == null)
                        return false;

                    var trees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SkuTree>>(product.SkuTree);
                    var tree = trees[0].V.Find(t => t.Id == sku.S1);
                    if (tree.ImgUrl.IsNotNull())
                        newItem.Image = tree.ImgUrl;

                    if (sku.Key1.IsNotNull())
                        newItem.SkuText += sku.Key1 + ":" + sku.Name1 + ";";
                    if (sku.Key2.IsNotNull())
                        newItem.SkuText += sku.Key2 + ":" + sku.Name2 + ";";
                    if (sku.Key3.IsNotNull())
                        newItem.SkuText += sku.Key3 + ":" + sku.Name3 + ";";

                    newItem.Price = sku.Price;
                }
                else
                {
                    newItem.Price = product.Price;
                }

                await this._engine.InsertOrUpdate<MemberCarItem>().SetSource(newItem).ExecuteAffrowsAsync();
            }
            else
            {
                item.Num += carItem.Num;
                item.Freight = product.Freight;
                item.FreightStep = product.FreightStep;
                if (carItem.SkuId.IsNotNull())
                {
                    var sku = await this._engine.Select<ProductSku>().Where(t => t.Id == carItem.SkuId).FirstAsync();
                    if (sku == null)
                        return false;
                    item.Price = sku.Price;
                }
                else
                {
                    item.Price = product.Price;
                }
                await this._engine.Update<MemberCarItem>().SetDto(new { item.Id, item.Num, item.Price, item.Freight, item.FreightStep }).ExecuteAffrowsAsync();
            }
            return true;
        }

        ///// <summary>
        ///// 修改购物车商品（sku/数量）
        ///// </summary>
        ///// <param name="product"></param>
        ///// <returns></returns>
        //public Task<bool> ChangeShoppingCartProduct(ShoppingCartProduct product)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 移除购物车商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> RemoveShoppingCartProduct(ShoppingCartProduct product, string memberId)
        {
            await this._engine.Delete<MemberCarItem>().Where(t => t.ProductId == product.PId && t.SkuId == product.SkuId && t.CarId == memberId).ExecuteAffrowsAsync();
            return true;
        }

        /// <summary>
        /// 下单后移出购物车
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="product"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveShoppingCartProductAfterTrade(IFreeSql context, string pId, string skuId, string memberId)
        {
            await context.Delete<MemberCarItem>().Where(t => t.ProductId == pId && t.SkuId == skuId && t.CarId == memberId).ExecuteAffrowsAsync();
            return true;
        }
    }
}
