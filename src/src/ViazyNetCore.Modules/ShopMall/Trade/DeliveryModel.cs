using System;
using System.Collections.Generic;
using System.Text;

namespace ViazyNetCore.Modules.ShopMall
{
    /// <summary>
    /// 发货实体
    /// </summary>
    public class DeliveryModel
    {
        public string LogisticsId { get; set; }

        public decimal LogisticsFee { get; set; }
        public string LogisticsCode { get; set; }
        public string LogisticsCompany { get; set; }

        public AddressModel Address { get;internal set; }
    }
}
