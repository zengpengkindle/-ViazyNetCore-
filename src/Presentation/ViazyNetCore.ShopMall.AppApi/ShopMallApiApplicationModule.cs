using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.ShopMall.Manage.Application;

namespace ViazyNetCore.ShopMall.AppApi
{
    [DependsOn(typeof(ShopMallManageModule)
        , typeof(AspNetCoreMvcModule))]
    public class ShopMallApiApplicationModule : InjectionModule
    {

    }
}
