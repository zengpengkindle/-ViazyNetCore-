using System;
using System.Collections.Generic;
using System.Text;

namespace ViazyNetCore.Modules.ShopMall
{
    public class BeforeCreateTradeModel
    {
        public decimal TotalMoney { get; set; }

        public List<BeforeShopTrade> ShopTrades { get; set; }

        public List<KeyValuePair<string, object>> Properties { get; set; }
    }

    public class BeforeShopTrade
    {
        public string ShopId { get; set; }

        public string ShopName { get; set; }

        public List<BeforeTradeItem> Items { get; set; }
    }

    public class BeforeTradeItem
    {
        public string ProductId { get; set; }

        public string SkuId { get; set; }

        public decimal Price { get; set; }

        public int Num { get; set; }

        public List<KeyValuePair<string, object>> Properties { get; set; }
    }
}
