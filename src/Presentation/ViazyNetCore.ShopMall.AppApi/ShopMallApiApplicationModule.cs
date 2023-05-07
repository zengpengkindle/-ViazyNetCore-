using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.AspNetCore;

namespace ViazyNetCore.ShopMall.AppApi
{
    [DependsOn(typeof(ShopMallApiApplicationModule), typeof(AspNetCoreMvcModule))]
    public class ShopMallApiApplicationModule : InjectionModule
    {

    }
}
