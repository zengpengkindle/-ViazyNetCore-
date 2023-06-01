using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.ShopMall.Manage.Application.ViewModel
{
    public class BatchDeliveryRes
    {
        public string[] TradeIds { get; set; }
        public DeliveryModel Delivery { get; set; }
    }
}
