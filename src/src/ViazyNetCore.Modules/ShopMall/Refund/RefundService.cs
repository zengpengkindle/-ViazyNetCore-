using System.Providers;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Modules.ShopMall
{
    public class RefundService
    {
        private readonly TradeService _tradeService;
        private readonly IFreeSql _engine;
        private readonly ILogger<RefundService> _logger;
        private readonly ILockProvider _lockProvider;

        public RefundService(IFreeSql engine, ILogger<RefundService> logger, TradeService tradeService, ILockProvider lockProvider)
        {
            this._engine = engine;
            this._logger = logger;
            this._tradeService = tradeService;
            this._lockProvider = lockProvider;
        }

        /// <summary>
        /// 发起申请时先获取需要的数据
        /// </summary>
        /// <param name="tradeId"></param>
        /// <returns></returns>
        //public async Task<IResult> FindRefundInfo(string memberId, string tradeId)
        //{
        //    var model = new RefundModel();

        //    var tradeResult = await this._tradeService.GetTradeDetail(memberId, tradeId);
        //    if(tradeResult.Fail())
        //        return tradeResult;
        //    var trade = tradeResult.Value;

        //    var shop = await this._engine.Select<Models.Shop>().Where(t => t.Id == trade.ShopId).FirstAsync();
        //    if(shop == null)
        //        throw new ApiException("店铺不存在");

        //    model.ShopId = shop.Id;
        //    model.ShopName = shop.ComponyName;
        //    model.ConsigneeAddress = shop.AddressTH;
        //    model.ConsigneeMobile = shop.Phone;
        //    model.ConsigneeName = shop.Contact;

        //    model.Trade = trade;

        //    var refunds = await (from r in this._engine.Select<RefundTrade>()
        //                         where r.TradeId == tradeId
        //                         orderby r.CreateTime descending
        //                         select new RefundTradeModel
        //                         {
        //                             ConsigneeAddress = "",
        //                             ConsigneeName = "",
        //                             ConsigneeMobile = "",
        //                             ConsigneeZip = "",
        //                             ExpressNo = "",
        //                             HandlingType = r.HandlingType,
        //                             Id = r.Id,
        //                             LogisticsFee = r.LogisticsFee,
        //                             LogisticsId = r.LogisticsId,
        //                             ReturnsAmount = r.ReturnsAmount,
        //                             ReturnsNo = r.ReturnsNo,
        //                             ReturnsType = r.ReturnsType,
        //                             SellerPunishFee = r.SellerPunishFee,
        //                             Status = r.Status
        //                         }).ToListAsync();
        //    foreach(var refund in refunds)
        //    {
        //        var orders = await (from o in this._engine.Select<RefundTradeOrder>()
        //                            where o.RefundTradeId == refund.Id 
        //                            select new RefundOrderModel
        //                            {
        //                                RefundNum = o.Num,
        //                                HandlingType = o.HandlingType,
        //                                OrderId = o.TradeOrderId,
        //                                SkuText = o.SkuText,
        //                                Status = o.Status,
        //                                Price = o.Price,
        //                                Title = o.ProductName
        //                            }).ToListAsync();
        //        refund.Orders = orders;
        //        var logs = await (from l in this._engine.Select<RefunTradeLog>()
        //                          where l.RefundTradeId == refund.Id && l.IsParent==true
        //                          orderby l.CreateTime descending
        //                          select new RefundLogModel
        //                          {
        //                              CreateTime = l.CreateTime,
        //                              LogisticsId = l.LogisticsId,
        //                              Message = l.Message,
        //                              Status = l.Status,
        //                              Title = l.Title,
        //                              Type = l.Type
        //                          }).ToListAsync();

        //        foreach(var log in logs)
        //        {
        //            var clogs = await (from l in this._engine.Select<RefunTradeLog>()
        //                              where l.ParentId == log.Id
        //                              orderby l.CreateTime descending
        //                              select new RefundLogModel
        //                              {
        //                                  CreateTime = l.CreateTime,
        //                                  LogisticsId = l.LogisticsId,
        //                                  Message = l.Message,
        //                                  Status = l.Status,
        //                                  Title = l.Title,
        //                                  Type = l.Type
        //                              }).ToListAsync();
        //            log.Children = clogs;
        //        }
        //        refund.Logs = logs;
        //    }
        //    model.RefundTrades = refunds;

        //    return Result.Success(model);
        //}

        public async Task<RefundTradeModel> FindRefundTradeInfo(string memberId, string tradeId)
        {
            var model = new RefundTradeModel();

            var trade = await this._engine.Select<ProductTrade>().Where(t => t.MemberId == memberId && t.Id == tradeId).FirstAsync();
            if (trade == null)
                throw new ApiException("订单不存在");

            model.TradeId = tradeId;
            model.ShopName = trade.ShopName;
            model.TradeStatus = trade.Status;
            model.ProductMoney = trade.ProductMoney;
            model.TotalFreight = trade.TotalFreight;

            var refundMoney = await this._engine.Select<RefundTrade>().Where(t => t.TradeId == tradeId && t.Status != RefundStatus.Cancel).SumAsync(t => t.ReturnsAmount);
            model.ApplyMoney = refundMoney;

            var orders = await this._engine.Select<ProductTradeOrder>()
                                .Where(o => o.ProductTradeId == tradeId)
                                .WithTempQuery(o => new RefundTradeOrderModel
                                {
                                    BuyNum = o.Num,
                                    OrderId = o.Id,
                                    Price = o.Price,
                                    SkuText = o.SkuText,
                                    Title = o.Title,
                                    ImgUrl = o.Image
                                }).ToListAsync();

            foreach (var order in orders)
            {
                var refundNum = await this._engine.Select<RefundTradeOrder>().Where(t => t.TradeOrderId == order.OrderId && t.Status != RefundStatus.Cancel).SumAsync(t => t.Num);
                order.RefundNum = (int)refundNum;


            }
            model.Orders = orders;

            return model;
        }

        /// <summary>
        /// 创建申请
        /// </summary>
        /// <returns></returns>
        public async Task CreateRefund(RefundSubmitModel model)
        {
            using (var context = this._engine.CreateUnitOfWork())
            {
                var trade = new RefundTrade();
                trade.Id = Snowflake<RefundTrade>.NextIdString();
                trade.TradeId = model.TradeId;
                trade.CreateTime = DateTime.Now;
                trade.Status = RefundStatus.Apply;

                trade.ReturnsType = model.ReturnsType;

                //var orders = new List<RefundTradeOrder>();

                foreach (var item in model.Orders)
                {
                    var tradeOrder = await _engine.Select<ProductTradeOrder>().Where(p => p.Id == item.TradeOrderId).ToOneAsync();
                    ///已申请或完成退换货的商品数
                    var refundNum = await _engine.Select<RefundTradeOrder>().Where(t => t.TradeOrderId == item.TradeOrderId && t.Status != RefundStatus.Cancel).SumAsync(t => t.Num);
                    //剩余的可申请退换货的商品数=购买数-已申请数
                    var exit = tradeOrder.Num - refundNum;

                    var order = new RefundTradeOrder();
                    order.Id = Snowflake<RefundTradeOrder>.NextIdString();
                    order.CreateTime = DateTime.Now;
                    order.Num = item.Num;
                    //如申请数量大于可申请数
                    if (item.Num > exit)
                    {
                        throw new ApiException("申请退货商品数量异常");
                    }
                    order.Price = tradeOrder.Price;
                    order.ProductName = tradeOrder.Title;
                    order.RefundTradeId = trade.Id;
                    order.SkuText = tradeOrder.SkuText;
                    order.Status = RefundStatus.Apply;
                    order.TradeOrderId = item.TradeOrderId;

                    await this._engine.Insert<RefundTradeOrder>().AppendData(order).ExecuteAffrowsAsync();
                    //orders.Add(order);
                }
                var log = new RefunTradeLog();
                log.Id = Snowflake<RefunTradeLog>.NextIdString();
                log.CreateTime = DateTime.Now;
                log.IsParent = true;
                log.Message = model.Message;
                log.RefundTradeId = trade.Id;
                log.Status = trade.Status;
                log.Title = model.Type + "-" + model.Select;
                log.Type = RefunTradeLogType.Buyer;

                await this._engine.Insert<RefundTrade>().AppendData(trade).ExecuteAffrowsAsync();

                await this._engine.Insert<RefunTradeLog>().AppendData(log).ExecuteAffrowsAsync();
                context.Commit();
            }
        }

        public Task CheckRefund()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户更新
        /// </summary>
        /// <returns></returns>
        public Task UpdateRefund()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 后台处理
        /// </summary>
        /// <returns></returns>
        public Task HandleRefund()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 对应退换订单消息发送
        /// </summary>
        /// <returns></returns>
        public Task SubmitMessage()
        {
            throw new NotImplementedException();
        }
    }


}
