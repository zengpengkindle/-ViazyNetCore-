using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    public class ShopPageQuery : Pagination
    {
        public string? Code { get; set; }
        public string? Name { get; set; }

        public PageLayout? Layout { get; set; }
        public PageType? Type { get; set; }
    }
}
