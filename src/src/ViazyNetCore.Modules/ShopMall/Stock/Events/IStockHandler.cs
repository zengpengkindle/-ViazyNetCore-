using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    public interface IStockHandler
    {
        Task OnGetProductStockAsync(StockModel stock);
    }

    public interface IStockChangeHandler
    {
        Task<bool> OnCreateProductStockAsync(IFreeSql context, string productId, string skuId, int initStock);
        Task OnUpdateLockStockAsync(IFreeSql context, string productId, string skuId, int lockNum);
        Task OnUpdateUnDeliverStockAsync(IFreeSql context, string productId, string skuId, int unDeliverNum);
        Task OnUpdateOutStockAsync(IFreeSql context, string productId, string skuId, int outStockNum);
        Task OnUpdateInStockAsync(IFreeSql context, string stockId, int inStockNum, string remark, string userId);
        Task OnUpdateRefundStockAsync(IFreeSql context, string productId, string skuId, int refundNum);
        Task OnUpdateExchangeStockAsync(IFreeSql context, string productId, string skuId, int exchangeNum);
    }
}
