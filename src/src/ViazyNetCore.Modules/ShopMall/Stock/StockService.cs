using System.Providers;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Modules.ShopMall
{
    public class StockService
    {
        private readonly IFreeSql _engine;
        private readonly ILogger<StockService> _logger;
        private readonly IEnumerable<IStockHandler> _stockHandler;
        private readonly IEnumerable<IStockChangeHandler> _stockChangeHandler;
        private readonly ILockProvider _lockProvider;

        public StockService(IFreeSql engine, ILogger<StockService> logger, IEnumerable<IStockHandler> stockHandler, IEnumerable<IStockChangeHandler> stockChangeHandler, ILockProvider lockProvider)
        {
            this._engine = engine;
            this._logger = logger;
            this._stockHandler = stockHandler;
            this._stockChangeHandler = stockChangeHandler;
            this._lockProvider = lockProvider;
        }

        /// <summary>
        /// 获取商品的库存信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<StockModel> FindProductStock(string productId)
        {
            var stock = this._engine.Select<ProductStock>();
            var product = await this._engine.Select<Product>().Where(t => t.Id == productId).FirstAsync();
            if (product == null)
                return null;

            var result = new StockModel();
            result.ProductId = productId;
            result.Title = product.Title;
            result.ImgUrl = product.Image;
            if (product.OpenSpec)
            {
                var skus = await stock.From<ProductSku>().InnerJoin((t, s) => s.Id == t.ProductSkuId && s.ProductId == productId)
                                  .WithTempQuery((t, s) => new StockSkuModel
                                  {
                                      Id = t.Id,
                                      ProductSkuId = s.Id,
                                      SkuText = s.Key1 + ':' + s.Name1 + ';' + s.Key2 + ':' + s.Name2 + ';' + s.Key3 + ':' + s.Name3 + ';',
                                      InStock = t.InStock,
                                      Lock = t.Lock,
                                      //UnDeliver = t.UnDeliver,
                                      OutStock = t.OutStock,
                                      SellNum = t.SellNum,
                                      Refund = t.Refund,
                                      Exchange = t.Exchange
                                  }).ToListAsync();
                result.Skus = skus;
                foreach (var sku in skus)
                {
                    result.InStock += sku.InStock;
                    result.Lock += sku.Lock;

                    result.OutStock += sku.OutStock;
                    result.SellNum += sku.SellNum;
                    result.Refund += sku.Refund;
                    result.Exchange += sku.Exchange;
                }
            }
            else
            {
                var item = await stock.Where(t => t.ProductId == productId).FirstAsync();
                if (item == null)
                    return null;
                result = item.CopyTo<StockModel>();
            }
            if (_stockHandler != null)
            {
                foreach (var handler in _stockHandler)
                {
                    await handler.OnGetProductStockAsync(result);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取某个规格商品库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public Task FindSkuStock(string productId, string skuId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取商品的库存操作记录
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<StockLogModel> GetStockUpdateLog(StockPageArgments args)
        {
            if (args.ProductId.IsNull())
                throw new ApiException("数据异常");

            var query = this._engine.Select<ProductStock>().From<Product, ProductSku>()
                        .InnerJoin((s, p, k) => p.Id == s.ProductId)
                        .LeftJoin((s, p, k) => k.Id == s.ProductSkuId)
                        .Where((s, p, k) => p.Id == args.ProductId)
                        .WithTempQuery((s, p, k) => new StockLogModel
                        {
                            StockId = s.Id,
                            ProductId = p.Id,
                            SkuId = k.Id,
                            Title = p.Title,
                            SkuText = k.Key1 + ':' + k.Name1 + ';' + k.Key2 + ':' + k.Name2 + ';' + k.Key3 + ':' + k.Name3 + ';',
                            ImgUrl = p.Image,
                            InStock = s.InStock,
                            Lock = s.Lock,
                            OutStock = s.OutStock,
                            Refund = s.Refund,
                            SellNum = s.SellNum,
                            Exchange = s.Exchange
                        });
            if (args.SkuId.IsNotNull())
                query = query.Where(q => q.SkuId == args.SkuId);
            var models = await query.ToListAsync();
            if (models.Count == 0)
                throw new ApiException("未找到商品的库存数据");


            if (models.Count == 1)
            {
                var logs = await this._engine.Select<ProductStockUpdateLog>()
                                  .Where(l => l.StockId == models[0].StockId)
                                  .OrderByDescending(l => l.CreateTime)
                                  .WithTempQuery(l => new StockLogItem
                                  {
                                      OldInStock = l.OldInStock,
                                      NewInStock = l.NewInStock,
                                      Amount = l.Amount,
                                      UserId = l.UserId,
                                      Remark = l.Remark,
                                      CreateTime = l.CreateTime
                                  }).ToListAsync();
                models[0].Logs = logs;
                if (models[0].SkuText.IsNotNull())
                    models[0].SkuText = models[0].SkuText.Replace(":;", "");

                return models[0];
            }
            else
            {
                var totalModel = new StockLogModel();
                totalModel.SkuStockLogModel = models;
                totalModel.ProductId = args.ProductId;
                totalModel.Title = models[0].Title;
                totalModel.ImgUrl = models[0].ImgUrl;
                foreach (var model in models)
                {
                    totalModel.InStock += model.InStock;
                    totalModel.Lock += model.Lock;
                    totalModel.OutStock += model.OutStock;
                    totalModel.Refund += model.Refund;
                    totalModel.SellNum += model.SellNum;
                    totalModel.Exchange += model.Exchange;
                    model.Logs = await this._engine.Select<ProductStockUpdateLog>()
                                        .Where(l => l.StockId == model.StockId)
                                        .OrderByDescending(l => l.CreateTime)
                                        .WithTempQuery(l => new StockLogItem
                                        {
                                            OldInStock = l.OldInStock,
                                            NewInStock = l.NewInStock,
                                            Amount = l.Amount,
                                            UserId = l.UserId,
                                            Remark = l.Remark,
                                            CreateTime = l.CreateTime
                                        }).ToListAsync();
                    model.SkuText = model.SkuText.Replace(":;", "");
                }

                return totalModel;
            }
        }

        /// <summary>
        /// 获取某个规格商品的库存操作记录
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public Task GetStockUpdateLog(string productId, string skuId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 新建商品初始化库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <param name="initStock"></param>
        /// <returns></returns>
        public async Task<bool> CreateProductStock(IFreeSql context, string productId, string skuId, int initStock)
        {
            try
            {
                //初始库存
                var stock = new ProductStock();
                stock.Id = Snowflake<ProductStock>.NextIdString();
                stock.ProductId = productId;
                stock.ProductSkuId = skuId;
                stock.InStock = initStock;
                stock.Lock = 0;
                //stock.UnDeliver = 0;
                stock.OutStock = 0;
                stock.SellNum = 0;
                stock.Refund = 0;
                stock.Exchange = 0;
                stock.CreateTime = DateTime.Now;
                stock.UpdateTime = DateTime.Now;
                await context.Insert<ProductStock>().AppendData(stock).ExecuteAffrowsAsync();


                if (this._stockChangeHandler != null)
                {
                    foreach (var handler in this._stockChangeHandler)
                    {
                        var result = await handler.OnCreateProductStockAsync(context, productId, skuId, initStock);
                        return result;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 库存锁仓数修改（已下单未付款由'在库'->'锁仓'||取消订单'锁仓'->'在库'）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <param name="lockNum">锁仓数变更数量，对应在库数-lockNum</param>
        /// <returns></returns>
        public async Task UpdateLockStock(IFreeSql context, string productId, string skuId, int lockNum)
        {
            try
            {
                if (lockNum == 0)
                    throw new ApiException("数据异常：锁仓变更数为0");

                var table = context.Select<ProductStock>().Where(t => t.ProductId == productId);
                if (skuId.IsNotNull())
                    table = table.Where(t => t.ProductSkuId == skuId);

                using (this._lockProvider.Lock(productId + skuId))
                {
                    var stock = await table.ToOneAsync();
                    //数据验证
                    if (lockNum > 0)
                    {
                        if (stock.InStock < lockNum)
                            throw new ApiException("商品库存不足");
                    }
                    else
                    {
                        if (stock.Lock < -lockNum)
                            throw new ApiException("商品库存锁仓数变更异常：当前锁仓数小于锁仓数减少数量");
                    }
                    //修改库存数据
                    stock.Lock += lockNum;
                    stock.InStock -= lockNum;
                    await context.Update<ProductStock>().SetDto(new { stock.Id, stock.Lock, stock.InStock }).ExecuteAffrowsAsync();

                    if (this._stockChangeHandler != null)
                    {
                        foreach (var handler in this._stockChangeHandler)
                        {
                            var result = await handler.OnUpdateLockStockAsync(context, productId, skuId, lockNum);
                            if (result.Fail())
                                return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApiException("商品库存锁仓数变更异常：" + ex.Message);
            }
        }


        /// <summary>
        /// (待发货数字段不用)待发货数量修改(付款后待发货由'锁仓'->'待发货'||申请退货成功后'待发货'->'在库'）此时商品已售出 对应sellNum+unDeliverNum
        /// </summary>
        /// <param name="context"></param>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <param name="unDeliverNum">待发货变更数量，为正对应锁仓数-unDeliverNum，为负对应在库数+unDeliverNum</param>
        /// <returns></returns>
        //public async Task<IResult> UpdateUnDeliverStock(IFreeSql context, string productId, string skuId, int unDeliverNum)
        //{
        //    try
        //    {
        //        if(unDeliverNum == 0)
        //            throw new ApiException("数据异常：待发货变更数为0");

        //        var table = context.Select<ProductStock>().Where(t => t.ProductId == productId);
        //        if(skuId.IsNotNull())
        //            table = table.Where(t => t.ProductSkuId == skuId);
        //        using(this._lockProvider.Lock<ProductStock>(productId + skuId))
        //        {
        //            var stock = await table.SingleAsync();

        //            if(unDeliverNum > 0)//待发货数增加，商品支付完成。
        //            {
        //                if(stock.Lock < unDeliverNum)
        //                    throw new ApiException("商品库存待发货数量变更异常：当前锁仓数小于待发货新增数量");
        //            }
        //            else//待发货数减少，支付完成且未发货商品申请退货成功时。
        //            {
        //                if(stock.UnDeliver < -unDeliverNum)
        //                    throw new ApiException("商品库存待发货数量变更异常：当前待发货数小于待发货减少数量");
        //            }

        //            stock.UnDeliver += unDeliverNum;
        //            //修改销量
        //            stock.SellNum += unDeliverNum;
        //            if(unDeliverNum > 0)
        //                stock.Lock -= unDeliverNum;
        //            else
        //                stock.InStock -= unDeliverNum;

        //            await context.Select<ProductStock>().ModifyAsync(new { stock.Id, stock.UnDeliver, stock.Lock, stock.InStock, stock.SellNum });

        //            if(this._stockChangeHandler != null)
        //            {
        //                foreach(var handler in this._stockChangeHandler)
        //                {
        //                    var result = await handler.OnUpdateUnDeliverStockAsync(context, productId, skuId, unDeliverNum);
        //                    if(result.Fail())
        //                        return result;
        //                }
        //            }
        //            return Result.Success();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new ApiException("商品库存待发货数量变更异常：" + ex.Message);
        //    }
        //}

        /// <summary>
        /// 出库数量修改
        /// </summary>
        /// <param name="context"></param>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <param name="outStockNum"></param>
        /// <returns></returns>
        public async Task UpdateOutStock(IFreeSql context, string productId, string skuId, int outStockNum)
        {
            try
            {
                if (outStockNum == 0)
                    throw new ApiException("数据异常：出库数变更数为0");

                var table = context.Select<ProductStock>().Where(t => t.ProductId == productId);
                if (skuId.IsNotNull())
                    table = table.Where(t => t.ProductSkuId == skuId);

                using (this._lockProvider.Lock(productId + skuId))
                {
                    var stock = await table.ToOneAsync();

                    if (outStockNum > 0)//出库->锁仓-outStockNum 已出库+outStockNum 验证待发货数
                    {
                        if (stock.Lock < outStockNum)
                            throw new ApiException("商品库存出库数数量变更异常：当前锁仓数小于出库数新增数量");
                    }
                    else//取消出库->锁仓-outStockNum 已出库+outStockNum 验证已出库数
                    {
                        if (stock.OutStock < -outStockNum)
                            throw new ApiException("商品库存出库数数量变更异常：当前出库数数小于出库数减少数量");
                    }
                    stock.OutStock += outStockNum;
                    stock.Lock -= outStockNum;

                    await context.Update<ProductStock>().SetDto(new { stock.Id, stock.Lock, stock.OutStock }).ExecuteAffrowsAsync();

                    if (this._stockChangeHandler != null)
                    {
                        foreach (var handler in this._stockChangeHandler)
                        {
                            var result = await handler.OnUpdateOutStockAsync(context, productId, skuId, outStockNum);
                            if (result.Fail())
                                return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApiException("商品库存出库数数量变更异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 修改在库数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="stockId"></param>
        /// <param name="inStockNum"></param>
        /// <param name="remark"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateInStock(IFreeSql context, string stockId, int inStockNum, string remark, string userId)
        {
            try
            {
                if (stockId.IsNull())
                    throw new ApiException("数据异常");
                if (inStockNum == 0)
                    throw new ApiException("数据异常：在库数变更数为0");
                if (remark.IsNull())
                    throw new ApiException("备注必填");

                var table = context.Select<ProductStock>().Where(t => t.Id == stockId);

                using (this._lockProvider.Lock(stockId))
                {
                    var stock = await table.ToOneAsync();

                    var stocklog = new ProductStockUpdateLog();
                    stocklog.Id = Snowflake<ProductStockUpdateLog>.NextIdString();
                    stocklog.Amount = inStockNum;
                    stocklog.CreateTime = DateTime.Now;
                    stocklog.NewInStock = stock.InStock + inStockNum;
                    stocklog.OldInStock = stock.InStock;
                    stocklog.Remark = remark;
                    stocklog.StockId = stock.Id;
                    stocklog.UserId = userId;
                    await context.Insert<ProductStockUpdateLog>().AppendData(stocklog).ExecuteAffrowsAsync();

                    if (inStockNum < 0 && stock.InStock < -inStockNum)
                        throw new ApiException("商品库存在库数量变更异常：当前在库数小于在库数减少数量");

                    //修改库存数
                    stock.InStock += inStockNum;
                    stock.UpdateTime = DateTime.Now;

                    await context.Update<ProductStock>().SetDto(new { stock.Id, stock.InStock, stock.UpdateTime }).ExecuteAffrowsAsync();

                    if (this._stockChangeHandler != null)
                    {
                        foreach (var handler in this._stockChangeHandler)
                        {
                            var result = await handler.OnUpdateInStockAsync(context, stockId, inStockNum, remark, userId);
                            if (result.Fail())
                                throw new ApiException(result.Message, result.Status);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }

        /// <summary>
        /// 记录退货件数-退货申请成功后 只累加
        /// </summary>
        /// <param name="context"></param>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <param name="refundNum"></param>
        /// <returns></returns>
        public async Task UpdateRefundStock(IFreeSql context, string productId, string skuId, int refundNum)
        {
            try
            {
                if (refundNum <= 0)
                    throw new ApiException("数据异常：退货件数变更数小于或等于0");

                var table = context.Select<ProductStock>().Where(t => t.ProductId == productId);
                if (skuId.IsNotNull())
                    table = table.Where(t => t.ProductSkuId == skuId);

                using (this._lockProvider.Lock(productId + skuId))
                {
                    var stock = await table.ToOneAsync();
                    stock.Refund += refundNum;


                    await context.Update<ProductStock>().SetDto(new { stock.Id, stock.Refund }).ExecuteAffrowsAsync();

                    if (this._stockChangeHandler != null)
                    {
                        foreach (var handler in this._stockChangeHandler)
                        {
                            var result = await handler.OnUpdateRefundStockAsync(context, productId, skuId, refundNum);
                            if (result.Fail())
                                return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApiException("商品库存退货件数数量变更异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 记录换货件数-换货申请成功后 只累加
        /// </summary>
        /// <param name="context"></param>
        /// <param name="productId"></param>
        /// <param name="skuId"></param>
        /// <param name="refundNum"></param>
        /// <returns></returns>
        public async Task UpdateExchangeStock(IFreeSql context, string productId, string skuId, int exchangeNum)
        {
            try
            {
                if (exchangeNum <= 0)
                    throw new ApiException("数据异常：换货件数变更数小于或等于0");

                var table = context.Select<ProductStock>().Where(t => t.ProductId == productId);
                if (skuId.IsNotNull())
                    table = table.Where(t => t.ProductSkuId == skuId);

                using (this._lockProvider.Lock(productId + skuId))
                {
                    var stock = await table.ToOneAsync();


                    stock.Exchange += exchangeNum;


                    await context.Update<ProductStock>().SetDto(new { stock.Id, stock.Exchange }).ExecuteAffrowsAsync();

                    if (this._stockChangeHandler != null)
                    {
                        foreach (var handler in this._stockChangeHandler)
                        {
                            var result = await handler.OnUpdateExchangeStockAsync(context, productId, skuId, exchangeNum);
                            if (result.Fail())
                                return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApiException("商品库存换货件数数量变更异常：" + ex.Message);
            }
        }
    }
}
