using System.Providers;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
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
        /// <summary>
        /// 流程处理
        /// </summary>
        /// <returns></returns>
        public async Task HandleRefund(RefundParamsModel model)
        {
            using (var context = this._engine.CreateUnitOfWork())
            {
                //当前流程
                var NowStep = await this._engine.Select<RefundStepLog>().Where(t => t.Id == model.NowStepId).ToOneAsync();
                if (NowStep == null)
                    throw new ApiException("退款流程异常");

                //验证流程是否已处理
                if (NowStep.NextStepLogId.IsNotNull())
                    throw new ApiException("退款流程异常");

                //当前流程配置
                var NowStepConfig = await _engine.Select<RefundStepConfig>().Where(t => t.Id == NowStep.StepId).ToOneAsync();
                if (NowStepConfig == null)
                    throw new ApiException("退款步骤异常");

                //验证下一步是否符合配置
                if (!NowStepConfig.NextStepIds.Contains(model.NextStepConfigId))
                    throw new ApiException("退款步骤异常");

                //下一步流程配置
                var NextStepConfig = await _engine.Select<RefundStepConfig>().Where(t => t.Id == model.NextStepConfigId).ToOneAsync();
                if (NextStepConfig == null)
                    throw new ApiException("退款步骤异常");
                if (NextStepConfig.SetRefundTradeStatus == RefundStatus.Cancel && NowStep.HandleUserType == RefundTradeLogType.Seller)
                {
                    if (model.Message.IsNull())
                        throw new ApiException("退款步骤异常");
                }

                //流程对应退换单
                var trade = await _engine.Select<RefundTrade>().Where(t => t.Id == NowStep.RefundTradeId).ToOneAsync();
                if (trade == null)
                    throw new ApiException("退款步骤异常");

                //退换单数据填写根据流程下一步的配置确定
                if (NextStepConfig.ShowFinance)//填写有关财务信息-
                {
                    trade.LogisticsFee = model.LogisticFee;
                    trade.ReturnsAmount = model.ReturnsAmount;
                    trade.SellerPunishFee = model.SellerPunishFee;
                }
                if (NextStepConfig.ShowLogistic)//填写有关的快递信息
                {
                    switch (NowStep.HandleUserType)//根据当前流程操作人判断应填写信息
                    {
                        case RefundTradeLogType.Seller:
                            trade.ConsigneeExpressNo = model.LogisticId;
                            trade.ConsigneeLogisticsName = model.LogisticName;
                            break;
                        case RefundTradeLogType.Buyer:
                            trade.ReturnLogisticsName = model.LogisticName;
                            trade.ReturnExpressNo = model.LogisticId;
                            break;
                    }
                }
                if (NextStepConfig.ShowRefund)//填写有关的回寄地址
                {
                    trade.ConsigneeAddress = model.RefundAddress;
                    trade.ConsigneeMobile = model.RefundMobile;
                    trade.ConsigneeName = model.RefundName;
                    trade.ConsigneeZip = model.RefundPostal;
                }

                //进入下一步流程后的订单状态
                trade.Status = NextStepConfig.SetRefundTradeStatus;

                foreach (var orderParams in model.RefundOrderParams)
                {
                    await _engine.Update<RefundTradeOrder>().SetDto(new
                    {
                        Id = orderParams.RefundOrderId,
                        trade.Status,
                        HandlingTime = DateTime.Now,
                        orderParams.HandlingType
                    }).ExecuteAffrowsAsync();
                }

                //生成下一步
                var NextStep = new RefundStepLog
                {
                    Id = Snowflake<RefundStepLog>.NextIdString(),
                    CreateTime = DateTime.Now,
                    HandleUserType = NextStepConfig.HandleUserType,
                    Message = model.Message,//附加说明
                    PreStepLogId = NowStep.Id,
                    RefundTradeId = NowStep.RefundTradeId,
                    Remind = NextStepConfig.Remind,
                    StepId = NextStepConfig.Id,
                    StepIndex = NowStep.StepIndex + 1,
                    StepName = NextStepConfig.StepName
                };

                NowStep.HandleTime = DateTime.Now;
                NowStep.HandleUserId = model.UserId;
                NowStep.NextStepLogId = NextStep.Id;

                trade.NewStepLogId = NextStep.Id;
                await this._engine.Update<RefundTrade>().SetDto(new
                {
                    trade.Id,
                    trade.Status,
                    trade.NewStepLogId,
                    trade.LogisticsFee,
                    trade.ReturnsAmount,
                    trade.SellerPunishFee,
                    trade.ConsigneeExpressNo,
                    trade.ConsigneeLogisticsName,
                    trade.ReturnLogisticsName,
                    trade.ReturnExpressNo,
                    trade.ConsigneeAddress,
                    trade.ConsigneeMobile,
                    trade.ConsigneeName,
                    trade.ConsigneeZip,
                    UpdateTime = DateTime.Now
                }).ExecuteAffrowsAsync();

                await this._engine.Update<RefundStepLog>().SetDto(new
                {
                    NowStep.Id,
                    NowStep.HandleTime,
                    NowStep.HandleUserId,
                    NowStep.NextStepLogId
                }).ExecuteAffrowsAsync();

                await this._engine.Insert<RefundStepLog>().AppendData(NextStep).ExecuteAffrowsAsync();

                context.Commit();
            }
        }

        /// <summary>
        /// 对应退换订单消息发送
        /// </summary>
        /// <returns></returns>
        public Task SubmitMessage()
        {
            throw new NotImplementedException();
        }

        public async Task<PageData<RefundListModel>> FindRefundTrades(RefundArgments args)
        {
            var fsql = this._engine.Select<RefundTrade>().From<ProductTrade>((rt, to) => rt.InnerJoin(r => r.TradeId == to.Id))
            .WhereIf(args.Id.IsNotNull(), (rt, to) =>rt.Id==args.Id)
            .WhereIf(args.MemberId.IsNotNull(), (rt, to) => to.MemberId == args.MemberId)
            //.WhereIf(args.HandleUserType.HasValue, (rt, to) => rt.Id == args.Id)
            .WhereIf(args.ShopId.IsNotNull(), (rt, to) => to.ShopId == args.ShopId)
            .WhereIf(args.Status.HasValue, (rt, to) => rt.Status == args.Status)
            //.WhereIf(args..IsNotNull(), (rt, to) => rt.Id == args.Id)
            ;
            var result = await fsql.WithTempQuery((p, to) => new RefundListModel
            {
                ApplyAmount = p.ReturnsAmount,
                ReturnsAmount = p.ReturnsAmount,
                CreateTime = p.CreateTime,
                UpdateTime = p.UpdateTime,
                Id = p.Id,
                HandleUserType = RefundTradeLogType.Seller,
                SellerPunishFee = p.SellerPunishFee,
                ShopId = to.ShopId,
                ShopName = to.ShopName,
                MemberId = to.MemberId,
                MemberName = to.MemberName,
                Status = p.Status,
                StepName = p.NewStepLogId

            }).ToPageAsync(args);
            return result;
        }
    }


}
