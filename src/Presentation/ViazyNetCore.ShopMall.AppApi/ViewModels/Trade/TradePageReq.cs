using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Model;

namespace ViazyNetCore.ShopMall.AppApi.ViewModels
{
    public class TradePageReq: Pagination
    {
        public TradeStatus? TradeStatus { get; set; }
    }
}
