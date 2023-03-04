using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Modules.ShopMall
{
    public class ProductService
    {
        private readonly IFreeSql _engine;
        private readonly ILogger<ProductService> _logger;
        private readonly IEnumerable<IProductHandler> _productHandlers;
        private readonly IEnumerable<IEditProductHanlder> _editProductHanlders;
        private readonly StockService _stockService;

        public ProductService(IFreeSql engine, ILogger<ProductService> logger, IEnumerable<IProductHandler> productHandlers, IEnumerable<IEditProductHanlder> editProductHanlders, StockService stockService)
        {
            this._engine = engine;
            this._logger = logger;
            this._productHandlers = productHandlers;
            this._editProductHanlders = editProductHanlders;
            this._stockService = stockService;
        }

        public async Task<PageData<Product>> FindProductAll(FindAllArguments args)
        {
            if (args.ShopId.IsNull())
                throw new ApiException("商店编号不能为空");
            var table = this._engine.Select<Product>().Where(t => t.Status != ProductStatus.Delete);

            if (args.ShopId != "111111")//假定官方自营店Id为"111111".官方可查所有店铺商品
                table = table.Where(s => s.ShopId == args.ShopId);

            if (args.TitleLike.IsNotNull())
                table = table.Where(t => t.Title.Contains(args.TitleLike));

            if (args.CatName.IsNotNull())
                table = table.Where(t => t.CatName == args.CatName);
            if (args.IsHidden.HasValue)
                table = table.Where(t => t.IsHidden == args.IsHidden);
            if (args.Status.HasValue)
                table = table.Where(t => t.Status == args.Status);


            if (args.CreateTimes?.Length == 2) table = table.Where(t => t.CreateTime >= args.CreateTimes[0] && t.CreateTime < args.CreateTimes[1].ToEndTime());

            var query = from t in table
                        orderby t.CreateTime descending
                        select t;
            var value = query.ToPage(args.PageNumber, args.PageSize);

            if (this._productHandlers != null)
            {
                foreach (var handler in this._productHandlers)
                {
                    var result = await handler.OnFindProductAllAsync(args);
                    if (result.Fail())
                    {
                        throw new ApiException(result.Message, result.Status);
                    }
                }
            }

            return value;
        }

        public async Task<ProductModel> FindProductDetail(string id)
        {
            var product = await this._engine.Select<Product>().Where(p => p.Id == id).FirstAsync();
            if (product == null)
                return null;
            var result = product.CopyTo<ProductModel>();

            var stock = await _stockService.FindProductStock(id);

            result.Stock = stock;

            result.Skus = new ProductSkuModel();
            result.Skus.Hide_stock = false;
            result.Skus.Collection_id = product.Id;

            result.Skus.Price = product.Price;
            result.Skus.Stock_num = result.Stock.InStock;
            result.Skus.None_sku = !result.OpenSpec;

            if (product.OpenSpec)
            {
                result.Skus.Tree = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SkuTree>>(product.SkuTree);

                result.Skus.List = await this._engine.Select<ProductSku>()
                                          .Where(s => s.ProductId == id)
                                          .WithTempQuery(s => new SkuModel
                                          {
                                              Id = s.Id,
                                              Price = s.Price,
                                              Cost = s.Cost,
                                              S1 = s.S1,
                                              S2 = s.S2,
                                              S3 = s.S3,
                                              Key1 = s.Key1,
                                              Key2 = s.Key2,
                                              Key3 = s.Key3,
                                              Name1 = s.Name1,
                                              Name2 = s.Name2,
                                              Name3 = s.Name3

                                          }).ToListAsync();
                for (int i = 0; i < result.Skus.List.Count; i++)
                {
                    result.Skus.List[i].StockNum = stock.Skus.Find(t => t.ProductSkuId == result.Skus.List[i].Id).InStock;
                }
            }


            if (_productHandlers != null)
            {
                foreach (var handler in _productHandlers)
                {
                    await handler.OnGetProductAsync(result);
                }
            }

            return result;
        }

        public async Task<ProductInfoModel> FindProductInfo(string productId)
        {
            var product = await this._engine.Select<Product>().Where(p => p.Id == productId).FirstAsync();
            if (product == null)
                return null;

            var instock = await this._engine.Select<ProductStock>().Where(p => p.ProductId == productId).SumAsync(t => t.InStock);

            var result = new ProductInfoModel
            {
                ProductId = productId,
                SkuId = "",
                ShopId = product.ShopId,
                CatId = product.CatId,
                CatName = product.CatName,
                BrandId = product.BrandId,
                BrandName = product.BrandName,
                ShopName = product.ShopName,
                Title = product.Title,
                SubTitle = product.SubTitle,
                Image = product.Image,
                SubImage = product.SubImage,
                Detail = product.Detail,
                HasOuter = product.HasOuter,
                OuterType = product.OuterType,
                SkuText = "",
                Cost = product.Cost,
                RefundType = product.RefundType,
                Price = product.Price,
                InStock = (int)instock,
                Status = product.Status
            };

            if (_productHandlers != null)
            {
                foreach (var handler in _productHandlers)
                {
                    await handler.OnGetProductInfoAsync(result);
                }
            }

            return result;
        }

        public async Task<ProductInfoModel> FindProductInfo(string productId, string skuId)
        {
            var sku = await this._engine.Select<ProductSku>().Where(p => p.ProductId == productId && p.Id == skuId).FirstAsync();
            if (sku == null)
                return null;
            var product = await this._engine.Select<Product>().Where(p => p.Id == productId).FirstAsync();
            if (product == null)
                return null;

            var instock = await this._engine.Select<ProductStock>().Where(p => p.ProductId == productId && p.ProductSkuId == skuId).SumAsync(t => t.InStock);

            var result = new ProductInfoModel
            {
                ProductId = productId,
                SkuId = sku.Id,
                ShopId = product.ShopId,
                ShopName = product.ShopName,
                Title = product.Title,
                Image = product.Image,
                HasOuter = product.HasOuter,
                OuterType = product.OuterType,
                Status = product.Status,
                Cost = sku.Cost,
                RefundType = product.RefundType,
                InStock = (int)instock,
                Price = sku.Price,

            };

            result.SkuText = "";
            if (sku.S1.IsNotNull() && sku.S1 != "0")
            {
                result.SkuText += sku.Key1 + ":" + sku.Name1 + ";";
            }
            if (sku.S2.IsNotNull() && sku.S2 != "0")
            {
                result.SkuText += sku.Key2 + ":" + sku.Name2 + ";";
            }
            if (sku.S3.IsNotNull() && sku.S3 != "0")
            {
                result.SkuText += sku.Key3 + ":" + sku.Name3 + ";";
            }

            var tree = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SkuTree>>(product.SkuTree).Find(t => t.KeyValue == "s1");
            var skuTree = tree.V.Find(v => v.Id == sku.S1);
            if (sku.S1.IsNotNull() && skuTree != null && skuTree.ImgUrl.IsNotNull())
            {
                result.Image = skuTree.ImgUrl;
            }

            if (_productHandlers != null)
            {
                foreach (var handler in _productHandlers)
                {
                    await handler.OnGetProductInfoAsync(result);
                }
            }

            return result;
        }
        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task ProductSubmit(ProductModel item)
        {
            using (var context = this._engine.CreateUnitOfWork())
            {
                var product = item.CopyTo<Product>();
                product.SearchContent = $"{product.Title}_{product.SubTitle}_{product.BrandName}_{product.CatPath}_{product.Keywords}";
                product.ModifyTime = DateTime.Now;
                product.StatusChangeTime = DateTime.Now;
                if (!product.BrandId.IsNull())
                {
                    var brand = await _engine.Select<ProductBrand>().Where(p => p.Id == product.BrandId).ToOneAsync();
                    product.BrandName = brand.Name;
                }
                if (!product.CatId.IsNull())
                {
                    var cat = await _engine.Select<ProductCat>().Where(p => p.Id == product.CatId).ToOneAsync();
                    product.CatName = cat.Name;
                }

                product.SkuTree = Newtonsoft.Json.JsonConvert.SerializeObject(item.Skus.Tree);
                if (product.Id.IsNull())
                {
                    product.Id = Snowflake<Product>.NextIdString();
                    product.CreateTime = DateTime.Now;
                    product.Status = ProductStatus.OnSale;

                    if (product.OpenSpec && item.Skus.List?.Count > 0)
                    {
                        var max = item.Skus.List[0].Price;
                        foreach (var skuItem in item.Skus.List)
                        {
                            var sku = new ProductSku();
                            sku.Id = Snowflake<ProductSku>.NextIdString();
                            sku.Cost = skuItem.Cost;
                            sku.Price = skuItem.Price;
                            if (sku.Price > max)
                                max = sku.Price;
                            sku.ProductId = product.Id;

                            sku.S1 = skuItem.S1.IsNull() ? "0" : skuItem.S1;
                            sku.S2 = skuItem.S2.IsNull() ? "0" : skuItem.S2;
                            sku.S3 = skuItem.S3.IsNull() ? "0" : skuItem.S3;

                            sku.Key1 = skuItem.Key1;
                            sku.Key2 = skuItem.Key2;
                            sku.Key3 = skuItem.Key3;

                            sku.Name1 = skuItem.Name1;
                            sku.Name2 = skuItem.Name2;
                            sku.Name3 = skuItem.Name3;

                            var skuStockResult = await this._stockService.CreateProductStock(_engine, product.Id, sku.Id, skuItem.StockNum);
                            if (!skuStockResult)
                                throw new ApiException("创建初始sku库存失败");
                            await this._engine.Insert<ProductSku>().AppendData(sku).ExecuteAffrowsAsync();
                        }
                        product.Price = max;

                    }
                    else
                    {
                        product.OpenSpec = false;
                        var stockResult = await this._stockService.CreateProductStock(_engine, product.Id, "", item.Stock.InStock);
                        if (!stockResult)
                            throw new ApiException("创建初始库存失败");
                    }
                    await _engine.Insert<Product>().AppendData(product).ExecuteAffrowsAsync();

                    if (this._editProductHanlders != null)
                    {
                        foreach (var handler in this._editProductHanlders)
                        {
                            var result = await handler.OnAddProductAsync(item);
                            if (result.Fail())
                            {
                                return result;
                            }
                        }
                    }
                    context.Commit();

                }
                else
                {
                    if (product.OpenSpec)
                    {
                        var max = item.Skus.List[0].Price;
                        var skuCheckItem = await this._engine.Select<ProductSku>().Where(s => s.ProductId == product.Id).FirstAsync();
                        if (skuCheckItem == null)
                            throw new ApiException("数据异常。开启sku商品没有找到sku数据");
                        var check = CheckSkuUpdate(skuCheckItem, item.Skus.List[0]);
                        if (!check)
                            throw new ApiException("不能变更商品已有规格属性。如有需要请新增商品");
                        foreach (var skuItem in item.Skus.List)
                        {

                            if (skuItem.Id.IsNull())
                            {
                                var sku = new ProductSku();
                                sku.Id = Snowflake<ProductSku>.NextIdString();
                                sku.Cost = skuItem.Cost;
                                sku.Price = skuItem.Price;
                                if (sku.Price > max)
                                    max = sku.Price;
                                sku.ProductId = product.Id;

                                sku.S1 = skuItem.S1;
                                sku.S2 = skuItem.S2;
                                sku.S3 = skuItem.S3;

                                sku.Key1 = skuItem.Key1;
                                sku.Key2 = skuItem.Key2;
                                sku.Key3 = skuItem.Key3;

                                sku.Name1 = skuItem.Name1;
                                sku.Name2 = skuItem.Name2;
                                sku.Name3 = skuItem.Name3;

                                var skuStockResult = await this._stockService.CreateProductStock(_engine, product.Id, sku.Id, skuItem.StockNum);
                                if (!skuStockResult)
                                    throw new ApiException("创建初始sku库存失败");
                                await _engine.Insert<ProductSku>().AppendData(sku).ExecuteAffrowsAsync();
                            }
                            else
                            {
                                await _engine.Update<ProductSku>().SetDto(new
                                {
                                    skuItem.Id,
                                    skuItem.Cost,
                                    skuItem.Price
                                }).ExecuteAffrowsAsync();
                            }
                            product.Price = max;
                        }
                    }
                    await _engine.Update<Product>().SetDto(new
                    {
                        product.Id,
                        product.Title,
                        product.SubTitle,
                        product.Keywords,
                        product.Description,
                        product.Cost,
                        product.Price,
                        product.Freight,
                        product.FreightStep,
                        product.FreightValue,
                        product.RefundType,
                        product.Image,
                        product.SkuTree,
                        product.SubImage,
                        product.Detail,
                        product.ModifyTime,
                        product.SearchContent,
                        product.IsHidden
                    }).ExecuteAffrowsAsync();
                    if (this._editProductHanlders != null)
                    {
                        foreach (var handler in this._editProductHanlders)
                        {
                            var result = await handler.OnUpdateProductAsync(item);
                            if (result.Fail())
                            {
                                return result;
                            }
                        }
                    }
                    context.Commit();

                }
            }
        }

        private bool CheckSkuUpdate(ProductSku check, SkuModel target)
        {
            if (check.Key1 != target.Key1 || check.Key2 != target.Key2 || check.Key3 != target.Key3)
                return false;

            return true;
        }

        public async Task ModifyProductStatus(string id, string shopId, ProductStatus status)
        {
            var product = await this._engine.Select<Product>().Where(t => t.Id == id && t.ShopId == shopId).FirstAsync();
            if (product == null)
                throw new ApiException("商品不存在");
            if (status == ProductStatus.Delete && product.Status == ProductStatus.OnSale)
                throw new ApiException("请先下架商品再删除");
            await this._engine.Update<Product>().SetDto(new { product.Id, status }).ExecuteAffrowsAsync();
            if (_editProductHanlders != null)
            {
                foreach (var handler in _editProductHanlders)
                {
                    var result = await handler.OnModifyProductStatusAsync(id, shopId, status);
                    if (result.Fail())
                        return result;
                }
            }

        }
    }
}
