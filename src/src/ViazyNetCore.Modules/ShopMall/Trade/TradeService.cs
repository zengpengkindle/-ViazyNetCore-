using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Providers;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.Internal;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class TradeService
    {
        private readonly IFreeSql _engine;
        private readonly ILogger<TradeService> _logger;
        private readonly StockService _stockService;
        private readonly ILockProvider _lockProvider;
        private readonly AddressService _addressService;
        private readonly IMemberService _memberService;
        private readonly ProductService _productService;
        private readonly CartService _cartService;
        private readonly PaymentService _paymentService;
        private readonly LogisticsService _logisticsService;
        private readonly IEnumerable<ITradeCreateHandler> _tradeCreateHandlers;
        private readonly IEnumerable<IOrderCreateHandler> _orderCreateHandlers;
        private readonly IEnumerable<IPayTradeHandler> _payTradeHandlers;
        private readonly IEnumerable<ITradeCancelHandler> _tradeCancelHandlers;
        private readonly IEnumerable<ITradeDeliveryHandler> _tradeDeliveryHandlers;
        private readonly IEnumerable<ITradeFinishHandler> _tradeFinishHandlers;

        public TradeService(IFreeSql engine, ILogger<TradeService> logger
            , StockService stockService
            , ILockProvider lockProvider
            , AddressService addressService
            , IMemberService memberService
            , ProductService productService
            , CartService cartService
            , PaymentService paymentService
            , LogisticsService logisticsService
            , IEnumerable<ITradeCreateHandler> tradeCreateHandlers
            , IEnumerable<IOrderCreateHandler> orderCreateHandlers
            , IEnumerable<IPayTradeHandler> payTradeHandlers
            , IEnumerable<ITradeCancelHandler> tradeCancelHandlers
            , IEnumerable<ITradeDeliveryHandler> tradeDeliveryHandlers
            , IEnumerable<ITradeFinishHandler> tradeFinishHandlers)
        {
            this._engine = engine;
            this._logger = logger;
            this._stockService = stockService;
            this._lockProvider = lockProvider;
            this._addressService = addressService;
            this._memberService = memberService;
            this._productService = productService;
            this._cartService = cartService;
            this._paymentService = paymentService;
            this._logisticsService = logisticsService;
            this._tradeCreateHandlers = tradeCreateHandlers;
            this._orderCreateHandlers = orderCreateHandlers;
            this._payTradeHandlers = payTradeHandlers;
            this._tradeCancelHandlers = tradeCancelHandlers;
            this._tradeDeliveryHandlers = tradeDeliveryHandlers;
            this._tradeFinishHandlers = tradeFinishHandlers;
        }

        /// <summary>
        /// 构建订单
        /// </summary>
        /// <returns></returns>
        public async Task<CreateTradeSetModel> BeforeCreateTradeAsync(string memberId, List<BeforeTradeItem> beforeTradeItems)
        {
            CreateTradeSetModel tradeSet = new CreateTradeSetModel();
            tradeSet.ShopTrades = new List<ShopTrade>();

            var ProductInfos = new List<ProductInfoModel>();
            foreach (var beforeItem in beforeTradeItems)
            {
                var productResult = await CheckProductStatus(beforeItem.ProductId, beforeItem.SkuId, beforeItem.Num);
                productResult.Num = beforeItem.Num;
                ProductInfos.Add(productResult);
            }

            var shops = ProductInfos.GroupBy(p => p.ShopId).Select(p => p.Key).ToArray();

            foreach (var shopId in shops)
            {

                decimal shopProductMoney = 0, shopTotalFreight = 0;

                var hasProductFreight = false;
                var hasShopFreight = false;

                var shopTrade = new ShopTrade()
                {
                    ShopId = shopId
                };
                var shopName = await this._engine.Select<Model.Shop>().Where(p => p.Id == shopId).WithTempQuery(p => p.ComponyName).FirstAsync();
                shopTrade.ShopName = shopName;
                shopTrade.Items = new List<TradeItem>();

                foreach (var product in ProductInfos.Where(p => p.ShopId == shopId))
                {

                    var tradeItem = new TradeItem()
                    {
                        Num = product.Num,
                        Price = product.Price,
                        PId = product.ProductId,
                        Pn = product.Title,
                        RefundType = product.RefundType,
                        SkuId = product.SkuId,
                        SkuText = product.SkuText,
                        ImgUrl = product.Image
                    };
                    shopTrade.Items.Add(tradeItem);

                    switch (product.FreightType)
                    {
                        case FreightType.Product:
                            hasProductFreight = true;
                            shopTotalFreight += product.FreightValue;
                            break;
                        case FreightType.Shop:
                            hasShopFreight = true;
                            break;
                        case FreightType.FreightStep:
                            hasProductFreight = true;
                            var freight = product.FreightValue;

                            if (product.Num - 1 > 0 && product.FreightStep > 0 && product.FreightValue > 0)
                                freight += (product.Num - 1) / product.FreightStep * product.FreightStepValue + ((product.Num - 1) % product.FreightStep > 0 ? product.FreightStepValue : 0);
                            shopTotalFreight += freight;

                            break;
                        case FreightType.Free:
                        default:
                            break;
                    }
                    shopProductMoney += tradeItem.Price * tradeItem.Num;
                    //插件处理
                    //if (this._orderCreateHandlers != null)
                    //{
                    //    foreach (var handler in this._orderCreateHandlers)
                    //    {
                    //        await handler.OnBeforeOrderCreateAsync(order);
                    //    }
                    //}
                }

                shopTrade.ProductMoney = shopProductMoney;
                shopTrade.TotalFreight = shopTotalFreight;

                if (hasShopFreight && !hasProductFreight)
                {//当有根据店铺定价及没有根据商品定价时计算。

                }
                shopTrade.TotalMoney = shopTrade.TotalFreight + shopTrade.ProductMoney;

                tradeSet.TotalMoney += shopTrade.TotalMoney;
                tradeSet.ShopTrades.Add(shopTrade);
            }
            return tradeSet;
        }

        private async Task<ProductInfoModel> CheckProductStatus(string productId, string skuId, int num)
        {
            if (num <= 0)
            {
                throw new ApiException("订单数量小于1");
            }
            ProductInfoModel product;
            if (skuId.IsNull() || productId == skuId)//前端如果productId==skuId时为无sku商品
            {
                product = await this._productService.FindProductInfo(productId);
            }
            else
            {
                product = await this._productService.FindProductInfo(productId, skuId);
            }
            if (product == null)
                throw new ApiException("未找到该商品");

            if (product.Status != ProductStatus.OnSale)
                throw new ApiException("该商品已下架");
            if (product.InStock < num)
                throw new ApiException("该商品库存不足");

            return product;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> CreateTradeSetAsync(string memberId, CreateTradeSetModel createTrade, string outerType = null)
        {
            using (_lockProvider.Lock("Create" + memberId))
            {
                var now = DateTime.Now;

                var address = await this._addressService.GetAddress(createTrade.AddressId);
                if (address == null)
                {
                    throw new ApiException("未选择收货地址。");
                }

                //var memberName = await this._memberService.GetMemberNameByMemberId(memberId);
                //if (memberName.IsNull())
                //{
                //    throw new ApiException("无效用户");
                //}

                var trades = new List<ProductTrade>();
                var tradeOrders = new List<ProductTradeOrder>();
                foreach (var shop in createTrade.ShopTrades)
                {
                    var trade = new ProductTrade()
                    {
                        Id = Snowflake<ProductTrade>.NextIdString(),
                        PaymentId = "",
                        MemberId = memberId,
                        //MemberName = memberName,
                        Status = TradeStatus.UnPay,
                        ShopId = shop.ShopId,
                        ShopName = shop.ShopName,

                        CreateTime = now,
                        StatusChangedTime = now,
                        BusinessType = outerType,

                        ReceiverProvince = address.Province,
                        ReceiverCity = address.City,
                        ReceiverDistrict = address.County,
                        ReceiverDetail = address.AddressDetail,
                        ReceiverName = address.Name,
                        ReceiverMobile = address.Tel,
                        Message = shop.Message
                    };

                    if (outerType != null)
                    {
                        trade.OuterType = outerType;
                        trade.HasOuter = true;
                    }

                    decimal shopProductMoney = 0, shopTotalFreight = 0;

                    var hasProductFreight = false;
                    var hasShopFreight = false;
                    foreach (var item in shop.Items)
                    {
                        var product = await CheckProductStatus(item.PId, item.SkuId, item.Num);

                        var order = new ProductTradeOrder()
                        {
                            Id = Snowflake<ProductTradeOrder>.NextIdString(),
                            ProductId = product.ProductId,
                            SkuId = product.SkuId,
                            SkuText = product.SkuText,
                            Cost = product.Cost,
                            Num = item.Num,
                            Image = product.Image,
                            Price = product.Price,
                            TotalMoney = product.Price * item.Num,
                            RefundType = product.RefundType,
                            Title = product.Title,
                            ProductTradeId = trade.Id
                        };
                        switch (product.FreightType)
                        {
                            case FreightType.Product:
                                hasProductFreight = true;
                                shopTotalFreight += product.FreightValue;
                                break;
                            case FreightType.Shop:
                                hasShopFreight = true;
                                break;
                            case FreightType.FreightStep:
                                hasProductFreight = true;
                                var freight = product.FreightValue;

                                if (item.Num - 1 > 0 && product.FreightStep > 0 && product.FreightValue > 0)
                                    freight += (item.Num - 1) / product.FreightStep * product.FreightStepValue + ((item.Num - 1) % product.FreightStep > 0 ? product.FreightStepValue : 0);
                                shopTotalFreight += freight;

                                break;
                            case FreightType.Free:
                            default:
                                break;
                        }

                        //插件处理
                        if (this._orderCreateHandlers != null)
                        {
                            foreach (var handler in this._orderCreateHandlers)
                            {
                                await handler.OnAfterOrderCreateAsync(order);
                            }
                        }

                        shopProductMoney += order.TotalMoney;
                        tradeOrders.Add(order);
                    }

                    //插件处理
                    if (_tradeCreateHandlers != null)
                    {
                        foreach (var handler in _tradeCreateHandlers)
                        {
                            await handler.OnAfterTradeCreateAsync(trade);
                        }
                    }

                    if (hasShopFreight && !hasProductFreight)
                    {//当有根据店铺定价及没有根据商品定价时计算。

                    }

                    trade.ProductMoney = shopProductMoney;
                    trade.TotalFreight = shopTotalFreight;

                    trade.TotalMoney = trade.TotalFreight + trade.ProductMoney;

                    //tradeSet.TotalFreight += shopTotalFreight;
                    //tradeSet.TotalMoney += trade.TotalMoney;
                    //tradeSet.PayMoney += tradeSet.TotalMoney;

                    trades.Add(trade);
                }

                var tradeIds = trades.Select(p => p.Id).ToArray();
                using (var context = this._engine.CreateUnitOfWork())
                {
                    //await context.Select<ProductTradeSet>().AddAsync(tradeSet);
                    foreach (var trade in trades)
                    {
                        await this._engine.Insert<ProductTrade>().AppendData(trade).ExecuteAffrowsAsync();
                    };
                    foreach (var order in tradeOrders)
                    {
                        await _stockService.UpdateLockStock(_engine, order.ProductId, order.SkuId, order.Num);
                        await _cartService.RemoveShoppingCartProductAfterTrade(_engine, order.ProductId, order.SkuId, memberId);
                        await _engine.Insert<ProductTradeOrder>().AppendData(order).ExecuteAffrowsAsync();
                    };

                    context.Commit();
                }

                return tradeIds;
            }
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<PageData<TradeDetailModel>> FindTradeList(TradePageArgments args)
        {
            var table = this._engine.Select<ProductTrade>();

            if (args.MemberId.IsNotNull())
                table = table.Where(t => t.MemberId == args.MemberId);
            else if (args.Username.IsNotNull())
            {
                var memberId = await this._memberService.GetMemberIdByUsername(args.Username);
                if (memberId.IsNull())
                    return new PageData<TradeDetailModel>();
                table = table.Where(t => t.MemberId == memberId);
            }
            else if (args.NickNameLike.IsNotNull())
            {
                var memberIds = await this._memberService.GetMemberIdsByMemberNameLike(args.NickNameLike);
                if (memberIds == null || memberIds.Count() == 0)
                    return new PageData<TradeDetailModel>();

                table = table.Where(t => memberIds.Contains(t.MemberId));
            }

            if (args.PayMode.HasValue)
                table = table.Where(t => t.PayMode == args.PayMode);

            if (args.TradeId.IsNotNull())
                table = table.Where(t => t.Id == args.TradeId);
            if (args.TradeStatus.HasValue)
                table = table.Where(t => t.Status == args.TradeStatus);

            if (args.CreateTimes?.Length == 2)
            {
                switch (args.TimeType)
                {
                    default:
                        table = table.Where(t => t.CreateTime >= args.CreateTimes[0] && t.CreateTime < args.CreateTimes[1].ToEndTime());
                        break;
                }
            }

            var query = table.From<Shop>().InnerJoin((t, s) => s.Id == t.ShopId)
                .WhereIf(args.ShopId.IsNotNull(), (t, s) => s.Id == args.ShopId)
                .WhereIf(args.ShopName.IsNotNull(), (t, s) => s.ComponyName.Contains(args.ShopName))
                        .WithTempQuery((t, s) => new TradeDetailModel
                        {
                            Id = t.Id,
                            ShopId = s.Id,
                            ShopName = s.ComponyName,
                            Name = t.MemberName,
                            TradeStatus = t.Status,
                            PayMode = t.PayMode,
                            Message = t.Message,
                            CreateTime = t.CreateTime,
                            PayTime = t.PayTime,
                            ConsignTime = t.ConsignTime,
                            CompleteTime = t.CompleteTime,
                            StatusChangedTime = t.StatusChangedTime,

                            Totalfeight = t.TotalFreight,
                            TotalMoney = t.TotalMoney,
                            ProductMoney = t.ProductMoney,

                            ReceiverProvince = t.ReceiverProvince,
                            ReceiverCity = t.ReceiverCity,
                            ReceiverDistrict = t.ReceiverDistrict,
                            ReceiverDetail = t.ReceiverDetail,
                            ReceiverMobile = t.ReceiverMobile,
                            ReceiverName = t.ReceiverName,

                            LogisticsCode = t.LogisticsCode,
                            LogisticsCompany = t.LogisticsCompany,
                            LogisticsId = t.LogisticsId,
                            LogisticsFee = t.LogisticsCost
                        });

            var trades = await query.ToPageAsync(args);
            for (int i = 0; i < trades.Rows.Count(); i++)
            {
                var orders = await this._engine.Select<ProductTradeOrder>().Where(o => o.ProductTradeId == trades.Rows[i].Id).ToListAsync();

                trades.Rows[i].Count = orders.Count;
                for (int j = 0; j < orders.Count; j++)
                {
                    trades.Rows[i].Orders.Add(new TradeOrderModel
                    {
                        OrderId = orders[j].Id,
                        ImgUrl = orders[j].Image,
                        ProductName = orders[j].Title,
                        Price = orders[j].Price,
                        PId = orders[j].ProductId,
                        SkuId = orders[j].SkuId,
                        SkuText = orders[j].SkuText,
                        Num = orders[j].Num
                    });
                }

                trades.Rows[i].Address = new TradeAddress
                {
                    Id = "",
                    Address = trades.Rows[i].ReceiverProvince + trades.Rows[i].ReceiverCity + trades.Rows[i].ReceiverDistrict + trades.Rows[i].ReceiverDetail,
                    Tel = trades.Rows[i].ReceiverMobile
                };
                trades.Rows[i].Logistics = new TradeLogistics
                {
                    Name = trades.Rows[i].LogisticsCompany,
                    ExpressNo = trades.Rows[i].LogisticsCode,
                    CreateTime = trades.Rows[i].ConsignTime
                };
            }

            return trades;
        }

        public async Task<TradeDetailModel> GetTradeDetail(string memberId, string tradeId)
        {
            var table = this._engine.Select<ProductTrade>().Where(t => t.Id == tradeId);
            if (memberId.IsNotNull())
                table = table.Where(t => t.MemberId == memberId);
            //var member = this._engine.Select<Member>();

            var shop = this._engine.Select<Model.Shop>();

            var query = table.From<Shop>().InnerJoin((t, s) => s.Id == t.ShopId)
                        .WithTempQuery((t, s) =>
                         new TradeDetailModel
                         {
                             Id = t.Id,
                             ShopId = s.Id,
                             ShopName = s.ComponyName,

                             Name = t.MemberName,

                             TradeStatus = t.Status,
                             PayMode = t.PayMode,

                             Message = t.Message,

                             CreateTime = t.CreateTime,
                             PayTime = t.PayTime,
                             ConsignTime = t.ConsignTime,
                             CompleteTime = t.CompleteTime,
                             StatusChangedTime = t.StatusChangedTime,

                             Totalfeight = t.TotalFreight,
                             TotalMoney = t.TotalMoney,
                             ProductMoney = t.ProductMoney,

                             ReceiverProvince = t.ReceiverProvince,
                             ReceiverCity = t.ReceiverCity,
                             ReceiverDistrict = t.ReceiverDistrict,
                             ReceiverDetail = t.ReceiverDetail,
                             ReceiverMobile = t.ReceiverMobile,
                             ReceiverName = t.ReceiverName,

                             LogisticsCode = t.LogisticsCode,
                             LogisticsCompany = t.LogisticsCompany,
                             LogisticsId = t.LogisticsId,
                             LogisticsFee = t.LogisticsCost
                         });

            var result = await query.FirstAsync();

            if (result == null)
                throw new ApiException("订单不存在");

            var orders = await this._engine.Select<ProductTradeOrder>().Where(o => o.ProductTradeId == result.Id).ToListAsync();

            result.Count = orders.Count;
            for (int j = 0; j < orders.Count; j++)
            {
                result.Orders.Add(new TradeOrderModel
                {
                    OrderId = orders[j].Id,
                    ImgUrl = orders[j].Image,
                    ProductName = orders[j].Title,
                    Price = orders[j].Price,
                    PId = orders[j].ProductId,
                    SkuId = orders[j].SkuId,
                    SkuText = orders[j].SkuText,
                    Num = orders[j].Num
                });
            }

            result.Address = new TradeAddress
            {
                Address = result.ReceiverProvince + result.ReceiverCity + result.ReceiverDistrict + result.ReceiverDetail,
                Tel = result.ReceiverMobile
            };
            result.Logistics = new TradeLogistics
            {
                Name = result.LogisticsCompany,
                ExpressNo = result.LogisticsCode,
                CreateTime = result.ConsignTime
            };

            return result;
        }

        public async Task<TradeSetModel> GetTradesPayInfo(string[] tradeIds, string memberId)
        {
            var table = this._engine.Select<ProductTrade>().Where(t => tradeIds.Contains(t.Id));

            var query = table.From<Shop>().InnerJoin((t, s) => s.Id == t.ShopId)
                        .Where((t, s) => t.MemberId == memberId)
                        .WithTempQuery((t, s) =>
                         new TradeDetailModel
                         {
                             Id = t.Id,
                             ShopId = s.Id,
                             ShopName = s.ComponyName,

                             TradeStatus = t.Status,
                             PayMode = t.PayMode,

                             Message = t.Message,

                             CreateTime = t.CreateTime,
                             PayTime = t.PayTime,
                             ConsignTime = t.ConsignTime,
                             CompleteTime = t.CompleteTime,
                             StatusChangedTime = t.StatusChangedTime,

                             Totalfeight = t.TotalFreight,
                             TotalMoney = t.TotalMoney,
                             ProductMoney = t.ProductMoney,

                             ReceiverProvince = t.ReceiverProvince,
                             ReceiverCity = t.ReceiverCity,
                             ReceiverDistrict = t.ReceiverDistrict,
                             ReceiverDetail = t.ReceiverDetail,
                             ReceiverMobile = t.ReceiverMobile,
                             ReceiverName = t.ReceiverName,

                             LogisticsCode = t.LogisticsCode,
                             LogisticsCompany = t.LogisticsCompany,
                             LogisticsId = t.LogisticsId
                         });

            var trades = await query.ToListAsync();

            var set = new TradeSetModel();

            for (int i = 0; i < trades.Count; i++)
            {
                if (trades[i].TradeStatus == TradeStatus.UnPay)
                {

                    var orders = await this._engine.Select<ProductTradeOrder>().Where(o => o.ProductTradeId == trades[i].Id).ToListAsync();

                    trades[i].Count = orders.Count;
                    for (int j = 0; j < orders.Count; j++)
                    {
                        trades[i].Orders.Add(new TradeOrderModel
                        {
                            OrderId = orders[j].Id,
                            ImgUrl = orders[j].Image,
                            ProductName = orders[j].Title,
                            Price = orders[j].Price,
                            PId = orders[j].ProductId,
                            SkuId = orders[j].SkuId,
                            SkuText = orders[j].SkuText,
                            Num = orders[j].Num
                        });
                    }

                    set.TotalProductMoney += trades[i].ProductMoney;
                    set.TotalFreigh += trades[i].Totalfeight;
                    set.TotalMoney += trades[i].TotalMoney;

                    if (trades[i].CreateTime < set.CreateTime)
                        set.CreateTime = trades[i].CreateTime;

                    var address = trades[i].ReceiverProvince + trades[i].ReceiverCity + trades[i].ReceiverDistrict + trades[i].ReceiverDetail;
                    var tel = trades[i].ReceiverMobile;
                    if (i == 0)
                    {
                        set.Address = new TradeAddress
                        {
                            Id = "",
                            Address = address,
                            Tel = tel
                        };
                    }
                    else
                    {
                        if (set.Address.Address != address && set.Address.Tel != tel)
                            set.AddressFailCount++;
                    }
                    set.Trades.Add(trades[i]);
                }
                else
                {
                    set.NoUnPayCount++;
                }
            }

            return set;
        }

        /// <summary>
        /// 创建支付
        /// </summary>
        /// <returns></returns>
        public async Task<PayCreateValue> CreatePayTradeSetAsync(string[] tradeIds, Buyway buyway, PayMediaType payMediaType, string returnUrl)
        {
            var trades = await this._engine.Select<ProductTrade>()
                       .Where(t => tradeIds.Contains(t.Id))
                       .ToListAsync();

            if (trades == null || trades.Count() == 0)
                throw new ApiException("订单不存在");

            if (trades.Any(p => p.Status != TradeStatus.UnPay))
            {
                throw new ApiException("付款订单状态错误。");
            }
            var amount = trades.Sum(p => p.TotalMoney);

            var tradeViewModel = new TradeViewModel
            {
                MemberId = trades.First().MemberId,
                Body = "商城购买商品",
                Subject = "商城购买商品",
                ReturnUrl = returnUrl,
                TotalAmount = amount
            };
            var result = await this._paymentService.CreatePrePayment(buyway, payMediaType, tradeViewModel);

            var outTradeNo = result.BodyOrderId;

            var payMode = buyway == Buyway.AliPay ? PayMode.Alipay : PayMode.Wechat;
            await this._engine.Update<ProductTrade>().Where(t => tradeIds.Contains(t.Id))
                .SetDto(new { TradeSetId = outTradeNo, PayMode = payMode }).ExecuteAffrowsAsync();

            foreach (var trade in trades)
            {
                trade.PaymentId = outTradeNo;
                if (this._payTradeHandlers != null)
                {
                    foreach (var handler in this._payTradeHandlers)
                    {
                        await handler.OnBeforeCreatePayAsync(trade);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        public async Task CancelTradeSetAsync(string memberId, string[] tradeIds)
        {
            using (_lockProvider.Lock("Trade" + memberId))
            {
                var trades = await this._engine.Select<ProductTrade>()
                    .Where(t => t.MemberId == memberId && tradeIds.Contains(t.Id))
                    .ToListAsync();

                if (trades.Any(p => p.Status != TradeStatus.UnPay))
                {
                    throw new ApiException("付款订单状态错误。");
                }
                //执行插件
                var isCancel = false;
                if (this._tradeCancelHandlers != null)
                {
                    foreach (var handler in this._tradeCancelHandlers)
                    {
                        foreach (var trade in trades)
                        {
                            var result = await handler.OnTradeCancelAsync(trade);
                            if (!result)
                            {
                                isCancel = true;
                                break;
                            }
                        }
                    }
                }
                if (isCancel)
                    throw new ApiException("插件返回取消命令！");

                using (var context = this._engine.CreateUnitOfWork())
                {
                    await _engine.Update<ProductTrade>().Where(p => tradeIds.Contains(p.Id)).SetDto(
                            new
                            {
                                Status = TradeStatus.Close,
                                StatusChangedTime = DateTime.Now
                            }
                        ).ExecuteAffrowsAsync();
                    context.Commit();
                }
            }
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        public async Task CancelTradeAsync(string memberId, string tradeId)
        {
            using (_lockProvider.Lock("Trade" + tradeId))
            {
                var trade = await this._engine.Select<ProductTrade>()
                  .Where(t => t.MemberId == memberId && t.Id == tradeId)
                  .FirstAsync();

                if (trade == null)
                    throw new ApiException("订单不存在");

                if (trade.Status != TradeStatus.UnPay)
                    throw new ApiException("付款订单无法取消");

                trade.Status = TradeStatus.Close;
                trade.StatusChangedTime = DateTime.Now;

                //执行插件
                var isCancel = false;
                if (this._tradeCancelHandlers != null)
                {
                    foreach (var handler in this._tradeCancelHandlers)
                    {
                        var result = await handler.OnTradeCancelAsync(trade);
                        if (!result)
                        {
                            isCancel = true;
                            break;
                        }
                    }
                }
                if (isCancel)
                    throw new ApiException("订单取消失败！");

                await this._engine.Update<ProductTrade>().SetDto(
                   new
                   {
                       trade.Id,
                       trade.Status,
                       trade.StatusChangedTime
                   }
               ).ExecuteAffrowsAsync();
            }
        }

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <returns></returns>
        public async Task TradeDeliveryAsync(string tradeId, DeliveryModel deliveryModel)
        {
            using (_lockProvider.Lock("Trade" + tradeId))
            {
                var trade = await this._engine.Select<ProductTrade>()
                   .Where(t => t.Id == tradeId)
                   .FirstAsync();

                if (trade.Status != TradeStatus.UnDeliver)
                    throw new ApiException("订单非待发货");
                var isCancel = false;
                if (this._tradeDeliveryHandlers != null)
                {
                    foreach (var handler in this._tradeDeliveryHandlers)
                    {
                        var result = await handler.OnTradeDeliveryAsync(trade);
                        if (!result)
                        {
                            isCancel = true;
                            break;
                        }
                    }
                }
                if (isCancel)
                    throw new ApiException("订单取消失败！");

                trade.Status = TradeStatus.UnReceive;
                trade.StatusChangedTime = DateTime.Now;
                trade.ConsignTime = DateTime.Now;
                trade.LogisticsCode = deliveryModel.LogisticsCode;
                trade.LogisticsId = deliveryModel.LogisticsId;
                trade.LogisticsCost = deliveryModel.LogisticsFee;
                trade.LogisticsCompany = deliveryModel.LogisticsCompany;

                deliveryModel.Address = new AddressModel()
                {
                    Province = trade.ReceiverProvince,
                    City = trade.ReceiverCity,
                    County = trade.ReceiverDistrict,
                    Name = trade.ReceiverName,
                    Tel = trade.ReceiverMobile,
                    AddressDetail = trade.ReceiverDetail,
                };
                await this._logisticsService.Delivery(tradeId, deliveryModel);
                var orderItems = await this._engine.Select<ProductTradeOrder>().Where(p => p.ProductTradeId == tradeId).WithTempQuery(p => new { p.ProductId, p.SkuId, p.Num }).ToListAsync();

                using (var context = this._engine.CreateUnitOfWork())
                {
                    await _engine.Update<ProductTrade>().SetDto(
                       new
                       {
                           trade.Id,
                           trade.Status,
                           trade.StatusChangedTime,
                           trade.ConsignTime,
                           trade.LogisticsCode,
                           trade.LogisticsId,
                           trade.LogisticsCost,
                           trade.LogisticsCompany
                       }
                   ).ExecuteAffrowsAsync();

                    foreach (var order in orderItems)
                    {
                        await this._stockService.UpdateOutStock(_engine, order.ProductId, order.SkuId, order.Num);
                    }
                    context.Commit();
                }
            }
        }

        /// <summary>
        /// 修改发货信息
        /// </summary>
        /// <param name="tradeId"></param>
        /// <param name="deliveryModel"></param>
        /// <returns></returns>
        public async Task TradeDeliveryChangeAsync(string tradeId, DeliveryModel deliveryModel)
        {
            using (_lockProvider.Lock("Trade" + tradeId))
            {
                var trade = await this._engine.Select<ProductTrade>()
                   .Where(t => t.Id == tradeId)
                   .FirstAsync();

                if (trade.Status != TradeStatus.UnReceive)
                    throw new ApiException("非待收货订单不能修改发货信息");

                var isCancel = false;
                if (this._tradeDeliveryHandlers != null)
                {
                    foreach (var handler in this._tradeDeliveryHandlers)
                    {
                        var result = await handler.OnTradeDeliveryAsync(trade);
                        if (!result)
                        {
                            isCancel = true;
                            break;
                        }
                    }
                }
                if (isCancel)
                    throw new ApiException("订单取消失败！");

                trade.LogisticsCode = deliveryModel.LogisticsCode;
                trade.LogisticsId = deliveryModel.LogisticsId;
                trade.LogisticsCost = deliveryModel.LogisticsFee;
                trade.LogisticsCompany = deliveryModel.LogisticsCompany;

                deliveryModel.Address = new AddressModel()
                {
                    Province = trade.ReceiverProvince,
                    City = trade.ReceiverCity,
                    County = trade.ReceiverDistrict,
                    Name = trade.ReceiverName,
                    Tel = trade.ReceiverMobile,
                    AddressDetail = trade.ReceiverDetail,
                };
                await this._logisticsService.Delivery(tradeId, deliveryModel);


                await this._engine.Update<ProductTrade>().SetDto(
                       new
                       {
                           trade.Id,
                           trade.LogisticsCode,
                           trade.LogisticsId,
                           trade.LogisticsCost,
                           trade.LogisticsCompany
                       }
                   ).ExecuteAffrowsAsync();
            }
        }

        /// <summary>
        /// 修改收货地址
        /// </summary>
        /// <param name="tradeId"></param>
        /// <param name="deliveryModel"></param>
        /// <returns></returns>
        public async Task TradeAddressChangeAsync(string tradeId, string name, string mobile, string province, string city, string district, string detail)
        {
            using (_lockProvider.Lock("Trade" + tradeId))
            {
                var trade = await this._engine.Select<ProductTrade>()
                   .Where(t => t.Id == tradeId)
                   .FirstAsync();

                if (trade.Status != TradeStatus.UnReceive && trade.Status != TradeStatus.UnDeliver)
                    throw new ApiException("非待发货或待收货订单不能修改收货信息");

                var isCancel = false;
                if (this._tradeDeliveryHandlers != null)
                {
                    foreach (var handler in this._tradeDeliveryHandlers)
                    {
                        var result = await handler.OnTradeDeliveryAsync(trade);
                        if (!result)
                        {
                            isCancel = true;
                            break;
                        }
                    }
                }
                if (isCancel)
                    throw new ApiException("订单取消失败！");

                trade.ReceiverName = name;
                trade.ReceiverMobile = mobile;
                trade.ReceiverProvince = province;
                trade.ReceiverCity = city;
                trade.ReceiverDistrict = district;
                trade.ReceiverDetail = detail;

                var fee = 0M;
                if (trade.LogisticsCost.HasValue)
                    fee = trade.LogisticsCost.Value;

                if (trade.Status == TradeStatus.UnDeliver)
                {
                    var deliveryModel = new DeliveryModel()
                    {
                        LogisticsId = trade.LogisticsId,
                        LogisticsCode = trade.LogisticsCode,
                        LogisticsCompany = trade.LogisticsCompany,
                        LogisticsFee = fee
                    };

                    deliveryModel.Address = new AddressModel()
                    {
                        Province = trade.ReceiverProvince,
                        City = trade.ReceiverCity,
                        County = trade.ReceiverDistrict,
                        Name = trade.ReceiverName,
                        Tel = trade.ReceiverMobile,
                        AddressDetail = trade.ReceiverDetail,
                    };
                    await this._logisticsService.Delivery(tradeId, deliveryModel);
                }
                await this._engine.Update<ProductTrade>().Where(p => p.Id == trade.Id).SetDto(
                       new
                       {
                           trade.ReceiverName,
                           trade.ReceiverMobile,
                           trade.ReceiverProvince,
                           trade.ReceiverCity,
                           trade.ReceiverDistrict,
                           trade.ReceiverDetail
                       }
                   ).ExecuteAffrowsAsync();
            }
        }


        /// <summary>
        /// 订单交易完成
        /// </summary>
        /// <returns></returns>
        public async Task TradeFinishAsync(string tradeId)
        {
            using (_lockProvider.Lock("Trade" + tradeId))
            {
                var trade = await this._engine.Select<ProductTrade>()
                     .Where(t => t.Id == tradeId)
                     .FirstAsync();

                var isCancel = false;
                if (this._tradeFinishHandlers != null)
                {
                    foreach (var handler in this._tradeFinishHandlers)
                    {
                        var result = await handler.OnTradeFinishAsync(trade);
                        if (!result)
                        {
                            isCancel = true;
                            break;
                        }
                    }
                }
                if (isCancel)
                    throw new ApiException("订单设置完成失败！");

                trade.Status = TradeStatus.Success;
                trade.StatusChangedTime = DateTime.Now;

                await this._engine.Update<ProductTrade>().SetDto(
                   new
                   {
                       trade.Id,
                       trade.Status,
                       trade.StatusChangedTime
                   }
               ).ExecuteAffrowsAsync();
            }
        }
    }
}
