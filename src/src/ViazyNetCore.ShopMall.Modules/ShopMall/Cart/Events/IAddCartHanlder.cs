using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    public interface IAddCartHanlder
    {
        Task OnBeforeAddCartAsync(ShoppingCartProduct product);
        Task OnAfterAddCartAsync(ShoppingCartProduct product);
    }

    public interface IChangeCartHanlder
    {
        Task OnBeforChangeCartAsync(ShoppingCartProduct product);
        Task OnAfterChangeCartAsync(ShoppingCartProduct product);
    }

    public interface IRemoveCartHanlder
    {
        Task OnRemoveCartAsync(ShoppingCartProduct product);
    }

    public interface IGetCartHanlder
    {
        Task OnFindCartAsync(long memberId);
    }
}
