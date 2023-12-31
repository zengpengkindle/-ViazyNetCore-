﻿namespace ViazyNetCore.Model
{
    [Table(Name = "ShopMall.MemberPayment")]
    public class MemberPayment : EntityBase<string>
    {
        public long MemberId { get; set; }
        public string TradeType { get; set; }
        public Buyway PayType { get; set; }
        public string PayChannel { get; set; }
        public decimal Amount { get; set; }
        public ApplyStatus Status { get; set; }
        public DateTime StatusChangeTime { get; set; }
        public DateTime CreateTime { get; set; }
        public string ResponseResult { get; set; }
    }
}
