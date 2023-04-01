using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall.Models
{
    public class ProductCatDto : ProductCatAddDto
    {
        public string Id { get; set; }

    }

    public class ProductCatUpdateDto : ProductCatDto
    {
    }
}
