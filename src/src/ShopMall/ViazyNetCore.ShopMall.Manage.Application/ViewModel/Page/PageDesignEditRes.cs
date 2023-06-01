using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.ShopMall.Manage.Application.ViewModel
{
    public class PageDesignEditRes
    {
        public string Code { get; set; }
        public List<DesginItem> Items { get; set; }
    }
}
