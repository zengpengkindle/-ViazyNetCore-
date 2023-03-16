using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.Manage.WebApi.ViewModel
{
    public class BatchDeliveryModel
    {
        public string[] TradeIds { get; set; }
        public DeliveryModel Delivery { get; set; }
    }
}
