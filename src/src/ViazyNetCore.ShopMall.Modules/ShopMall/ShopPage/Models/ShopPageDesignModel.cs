using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ViazyNetCore.Modules.ShopMall
{
    public class DesginItem
    {
        public string Type { get; set; }
        public JObject? Value { get; set; }
    }
}
