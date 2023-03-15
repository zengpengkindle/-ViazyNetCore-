using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ViazyNetCore.Modules.ShopMall
{
    /// <summary>
    /// 订单查询用
    /// </summary>
    public class TradePageArgments : Pagination
    {
        public string MemberId { get; set; }

        public string TradeId { get; set; }

        public string NickNameLike { get; set; }

        public string Username { get; set; }

        public string ShopId { get; set; }

        public string ShopName { get; set; }

        public DateTime[] CreateTimes { get; set; }

        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }

        public TradeStatus? TradeStatus { get; set; }

        public int TimeType { get; set; }

        public PayMode? PayMode { get; set; }
    }

    public class TradeSetModel
    {
        public List<TradeDetailModel> Trades { get; set; } = new List<TradeDetailModel>();

        public decimal TotalProductMoney { get; set; } = 0;

        public decimal TotalFreigh { get; set; } = 0;

        public decimal TotalMoney { get; set; } = 0;

        public decimal TotalDiscount { get; set; } = 0;


        public DateTime CreateTime { get; set; } = DateTime.MaxValue;

        public int NoUnPayCount { get; set; } = 0;

        public int AddressFailCount { get; set; } = 0;

        public TradeAddress Address { get; set; } = new TradeAddress();
    }

    public class TradeDetailModel
    {
        public string Id { get; set; }

        public string ShopId { get; set; }

        public string ShopName { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public TradeStatus TradeStatus { get; set; }

        public PayMode PayMode { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示下单时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示付款时间。
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示发货时间。
        /// </summary>
        public DateTime? ConsignTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示完成时间。
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示订单状态变更时间。
        /// </summary>
        public DateTime StatusChangedTime { get; set; }

        public decimal Totalfeight { get; set; }

        public decimal TotalMoney { get; set; }

        public decimal ProductMoney { get; set; }

        public string Message { get; set; }

        public int Count { get; set; }

        [JsonProperty("items")]
        public List<TradeOrderModel> Orders { get; set; } = new List<TradeOrderModel>();

        public TradeAddress Address { get; set; } = new TradeAddress();

        /// <summary>
        /// 设置或获取一个值，表示收货地址所在的省份。
        /// </summary>
        public string ReceiverProvince { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货地址所在的城市。
        /// </summary>
        public string ReceiverCity { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货地址所在的区县。
        /// </summary>
        public string ReceiverDistrict { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示详细的收货地址。
        /// </summary>
        public string ReceiverDetail { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货人姓名。
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示收货人手机。
        /// </summary>
        public string ReceiverMobile { get; set; }

        public TradeLogistics Logistics { get; set; } = new TradeLogistics();

        /// <summary>
        /// 设置或获取一个值，表示物流公司编号。
        /// </summary>
        public string LogisticsId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流单号。
        /// </summary>
        public string LogisticsCode { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示物流公司名称。
        /// </summary>
        public string LogisticsCompany { get; set; }

        public decimal? LogisticsFee { get; set; }
        //totalfeight totalMoney count
    }

    public class TradeAddress
    {
        public string Id { get; set; }

        public string Tel { get; set; }
        /// <summary>
        /// address: '浙江省杭州市西湖区文三路 138 号东方通信大厦 7 楼 501 室'
        /// </summary>
        public string Address { get; set; }
    }

    /// <summary>
    /// 快递单号信息
    /// </summary>
    public class TradeLogistics
    {
        public string Name { get; set; }

        public string ExpressNo { get; set; }

        public DateTime? CreateTime { get; set; }
    }

    public class TradeOrderModel
    {
        public string OrderId { get; set; }

        public string ImgUrl { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [JsonProperty("pn")]
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string PId { get; set; }

        public string SkuId { get; set; }

        /// <summary>
        /// { name: '颜色:红色', ks: "123" }
        /// </summary>
        public string SkuText { get; set; }

        public int Num { get; set; }
    }
}
