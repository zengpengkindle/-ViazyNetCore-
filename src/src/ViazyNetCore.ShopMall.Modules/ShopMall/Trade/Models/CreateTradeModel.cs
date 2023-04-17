using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    public class CreateTradeSetModel
    {
        public decimal TotalMoney { get; set; }

        public int Num { get; set; }

        public string AddressId { get; set; }

        public List<ShopTrade> ShopTrades { get; set; }
    }

    public class ShopTrade
    {
        public string ShopId { get; set; }

        public string? ShopName { get; set; }

        public List<TradeItem> Items { get; set; }

        public string? Message { get; set; }

        /// <summary>
        /// 优惠券
        /// </summary>
        public string? VoucherId { get; set; }

        public decimal ProductMoney { get; set; }

        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 邮费
        /// </summary>
        public decimal TotalFreight { get; set; }
    }

    public class TradeItem
    {
        public string PId { get; set; }
        public string Pn { get; set; }
        public string SkuId { get; set; }
        public string SkuText { get; set; }

        public decimal Price { get; set; }

        public int Num { get; set; }

        public string? ImgUrl { get; set; }

        public RefundType RefundType { get; set; }
    }
}
