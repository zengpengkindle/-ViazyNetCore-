﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Manage.WebApi.ViewModel
{
    public class TradeAddressRequest
    {
        public string Id { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverMobile { get; set; }
        public string ReceiverProvince { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverDistrict { get; set; }
        public string ReceiverDetail { get; set; }
    }
}