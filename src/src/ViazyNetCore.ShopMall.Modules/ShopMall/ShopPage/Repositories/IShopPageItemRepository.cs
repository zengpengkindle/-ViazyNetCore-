﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall.Repository
{
    [Injection]
    public interface IShopPageItemRepository : IBaseRepository<ShopPageItem, long>
    {
        Task DeleteByCode(string code);
    }
}
