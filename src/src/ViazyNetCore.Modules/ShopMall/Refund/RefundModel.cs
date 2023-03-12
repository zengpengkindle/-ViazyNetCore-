using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Model;

namespace ViazyNetCore.Modules.ShopMall
{
    #region 废弃
    /*
    public class RefundModel
    {
        public string ShopId { get; set; }

        public string ShopName { get; set; }

        /// <summary>
        /// 退货收货人
        /// </summary>
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 退货人联系电话
        /// </summary>
        public string ConsigneeMobile { get; set; }

        /// <summary>
        /// 退货地址
        /// </summary>
        public string ConsigneeAddress { get; set; }

        public TradeDetailModel Trade { get; set; }

        public List<RefundTradeModel> RefundTrades { get; set; }
    }

    public class RefundTradeModel
    {
        public string Id { get; set; }

        public ReturnsType ReturnsType { get; set; }

        /// <summary>
        /// 退货单号
        /// </summary>
        public string ReturnsNo { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 退货收货人
        /// </summary>
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 退货人联系电话
        /// </summary>
        public string ConsigneeMobile { get; set; }

        /// <summary>
        /// 退货地址
        /// </summary>
        public string ConsigneeAddress { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示邮政编码。
        /// </summary>
        public string ConsigneeZip { get; set; }

        /// <summary>
        /// 物流公司编号
        /// </summary>
        public string LogisticsId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示产生的物流花费
        /// </summary>
        public decimal? LogisticsFee { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 处理结果：PUPAWAY:退货入库;REDELIVERY:重新发货;RECLAIM-REDELIVERY:不要求归还并重新发货; REFUND:退款; COMPENSATION:不退货并赔偿。
        /// </summary>
        public HandlingType? HandlingType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示处理状态 -1买家取消 0申请中 1等待买家退回 2买家已退，卖家等待收货 3卖家已收货，等待处理 4卖家已处理，等待买家确认 5流程结束。
        /// </summary>
        public RefundStatus Status { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal? ReturnsAmount { get; set; }

        /// <summary>
        /// 卖家承担运费
        /// </summary>
        public decimal? SellerPunishFee { get; set; }

        public List<RefundOrderModel> Orders { get; set; }

        public List<RefundLogModel> Logs { get; set; }
    }

    public class RefundOrderModel
    {
        public string OrderId { get; set; }

        public string Title { get; set; }

        public string SkuText { get; set; }

        public int RefundNum { get; set; }

        public decimal Price { get; set; }

        public RefundStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示 PUPAWAY:退货入库;REDELIVERY:重新发货;RECLAIM-REDELIVERY:不要求归还并重新发货; REFUND:退款; COMPENSATION:不退货并赔偿。
        /// </summary>
        public HandlingType? HandlingType { get; set; }
    }

    public class RefundLogModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 设置或获取一个值，表示标题（1申请退货；2申请换货；3拒绝退货；4拒绝换货；5等待收取退货；6等待处理；7处理完毕）。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示0卖家信息 1买家信息。
        /// </summary>
        public RefunTradeLogType Type { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示说明。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示附加信息-物流单号（买家退回商品时物流单号）。
        /// </summary>
        public string LogisticsId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示附加信息-退款单所处状态。
        /// </summary>
        public RefundStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        public List<RefundLogModel> Children { get; set; }
    }
    */
    #endregion

    #region 发起前

    /// <summary>
    /// 售后信息-订单信息
    /// </summary>
    public class RefundTradeModel
    {
        public string ShopName { get; set; }
        public string TradeId { get; set; }
        /// <summary>
        /// 商品金额
        /// </summary>
        public decimal ProductMoney { get; set; }
        /// <summary>
        /// 总运费
        /// </summary>
        public decimal TotalFreight { get; set; }
        /// <summary>
        /// 已申请的金额（包含已退和申请中）
        /// </summary>
        public decimal ApplyMoney { get; set; }
        public TradeStatus TradeStatus { get; set; }
        public List<RefundTradeOrderModel> Orders { get; set; }
    }

    /// <summary>
    /// 售后信息-订单子订单信息
    /// </summary>
    public class RefundTradeOrderModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string OrderId { get; set; }

        public string Title { get; set; }

        public string SkuText { get; set; }

        public int RefundNum { get; set; }

        public int BuyNum { get; set; }

        public decimal Price { get; set; }

        public string ImgUrl { get; set; }
    }
    #endregion

    #region 发起时

    /// <summary>
    /// 售后信息-提交用
    /// </summary>
    public class RefundSubmitModel
    {
        public string TradeId { get; set; }

        public string Type { get; set; }

        public string Select { get; set; }

        public string Message { get; set; }

        public decimal RefundMoney { get; set; }

        public ReturnsType ReturnsType { get; set; }

        public List<RefundSubmitOrderModel> Orders { get; set; }
    }

    /// <summary>
    /// 售后信息-提交用子单
    /// </summary>
    public class RefundSubmitOrderModel
    {
        public string TradeOrderId { get; set; }

        public int Num { get; set; }
    }
    #endregion

    public class RefundLogModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 设置或获取一个值，表示标题（1申请退货；2申请换货；3拒绝退货；4拒绝换货；5等待收取退货；6等待处理；7处理完毕）。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示0卖家信息 1买家信息。
        /// </summary>
        public RefunTradeLogType Type { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示说明。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示附加信息-物流单号（买家退回商品时物流单号）。
        /// </summary>
        public string LogisticsId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示附加信息-退款单所处状态。
        /// </summary>
        public RefundStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        public List<RefundLogModel> Children { get; set; }
    }
    #region 流程处理

    /// <summary>
    /// 列表搜索参数
    /// </summary>
    public class RefundArgments : Pagination
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string MemberId { get; set; }

        public string Id { get; set; }

        public string Username { get; set; }

        public string NickNameLike { get; set; }

        public string ShopId { get; set; }
        public string ShopName { get; set; }

        public RefundTradeLogType? HandleUserType { get; set; }

        public RefundStatus? Status { get; set; }

        public int TimeType { get; set; }

        public DateTime[] CreateTimes { get; set; }
    }

    /// <summary>
    /// 列表参数
    /// </summary>
    public class RefundListModel
    {
        public string Id { get; set; }

        public string MemberId { get; set; }

        public string MemberName { get; set; }

        public string ShopId { get; set; }

        public string ShopName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示步骤处理人类型 0商家1用户。
        /// </summary>
        public RefundTradeLogType HandleUserType { get; set; }

        public RefundStatus Status { get; set; }

        public string StepName { get; set; }

        public decimal ReturnsAmount { get; set; }

        public decimal ApplyAmount { get; set; }

        public decimal? SellerPunishFee { get; set; }

        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        public List<RefundListOrderModel> Orders { get; set; } = new List<RefundListOrderModel>();
    }

    public class RefundListOrderModel
    {
        public string ProductName { get; set; }

        public string SkuText { get; set; }

        public string ImgUrl { get; set; }

        public decimal Price { get; set; }

        public int Num { get; set; }
    }

    /// <summary>
    /// 售后详情
    /// </summary>
    public class RefundDetailModel
    {
        public string Id { get; set; }

        public string MemberId { get; set; }

        public string MemberName { get; set; }

        public string ShopId { get; set; }

        public string ShopName { get; set; }

        public string Title { get; set; }

        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        public RefundStatus Status { get; set; }

        public RefundTradeLogType HandleUserType { get; set; }

        /// <summary>
        /// 最新的处理流程记录编号
        /// </summary>
        public string NewStepLogId { get; set; }

        public string StepName { get; set; }

        public decimal ReturnsAmount { get; set; }

        public decimal ApplyAmount { get; set; }

        public decimal? SellerPunishFee { get; set; }

        /// <summary>
        /// 退货收货人
        /// </summary>
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 退货人联系电话
        /// </summary>
        public string ConsigneeMobile { get; set; }

        /// <summary>
        /// 退货地址
        /// </summary>
        public string ConsigneeAddress { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示邮政编码。
        /// </summary>
        public string ConsigneeZip { get; set; }

        /// <summary>
        /// 物流单号-用户回寄
        /// </summary>
        public string ReturnExpressNo { get; set; }

        /// <summary>
        /// 物流公司-用户回寄
        /// </summary>
        public string ReturnLogisticsName { get; set; }

        /// <summary>
        /// 物流单号-商家再寄出
        /// </summary>
        public string ConsigneeExpressNo { get; set; }

        /// <summary>
        /// 物流公司-商家再寄出
        /// </summary>
        public string ConsigneeLogisticsName { get; set; }

        public RefundParamsModel Params { get; set; }

        public List<RefundListOrderModel> Orders { get; set; } = new List<RefundListOrderModel>();
    }

    /// <summary>
    /// 业务流程参数
    /// </summary>
    public class RefundParamsModel
    {
        #region 共通
        public string RefundTradeId { get; set; }

        public string NowStepId { get; set; }

        public string NextStepConfigId { get; set; }

        public List<NextStepModel> NextSteps { get; set; }

        public string UserId { get; set; }

        public string Message { get; set; }

        public HandlingType? HandlingType { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示步骤处理人类型 0商家1用户。
        /// </summary>
        public RefundTradeLogType HandleUserType { get; set; }


        #endregion

        #region 回寄地址
        public string RefundAddress { get; set; }

        public string RefundMobile { get; set; }

        public string RefundName { get; set; }

        public string RefundPostal { get; set; }
        #endregion

        #region 快递信息
        public string LogisticId { get; set; }

        public string LogisticName { get; set; }
        #endregion

        #region 财务信息
        public decimal ReturnsAmount { get; set; }

        public decimal SellerPunishFee { get; set; }

        public decimal LogisticFee { get; set; }
        #endregion

        public List<RefundOrderParamsModel> RefundOrderParams { get; set; } = new List<RefundOrderParamsModel>();

    }

    /// <summary>
    /// 子参数
    /// </summary>
    public class RefundOrderParamsModel
    {
        public string RefundOrderId { get; set; }

        public HandlingType HandlingType { get; set; }
    }

    public class NextStepModel
    {
        public string NextStepConfigId { get; set; }

        public string NextStepName { get; set; }

        /// <summary>
        /// 是否需要填写参数-用户回寄地址信息
        /// </summary>
        public bool ShowRefund { get; set; }

        /// <summary>
        /// 是否需要填写参数-快递信息
        /// </summary>
        public bool ShowLogistic { get; set; }

        /// <summary>
        /// 是否需要填写参数-退货产生的财务信息
        /// </summary>
        public bool ShowFinance { get; set; }
    }
    #endregion
}
