using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.ShopMall.AppApi
{
    [DependsOn(typeof(ShopMallApiApplicationModule), typeof(AspNetCoreModule))]
    public class ShopMallApiApplicationModule : InjectionModule
    {

    }
}
