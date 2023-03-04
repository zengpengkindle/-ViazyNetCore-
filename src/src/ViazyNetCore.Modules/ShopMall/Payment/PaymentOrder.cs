using System;
using System.Collections.Generic;
using System.Text;

namespace ViazyNetCore.Modules.ShopMall.Payment
{
    public class PaymentOrder
    {
        public string Id { get; set; }

        public string MemberId { get; set; }

        public PayMode PayMode { get; set; }

        public PayStatus PayStatus { get; set; }

        public PayMediaType PayMediaType { get; set; }

        public int TotalMoney { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示订单超时时间，小于 0 表示不限超时时间。
        /// </summary>
        public int TimeoverHour { get; set; }

        public string OuterId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? NotifyTime { get; set; }
        public string NotifyMessage { get; set; }

        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// 设置或获取一个值，表示订单状态变更时间。
        /// </summary>
        public DateTime StatusChangedTime { get; set; }
    }
}
