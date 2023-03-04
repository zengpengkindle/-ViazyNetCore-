namespace ViazyNetCore.Modules.ShopMall
{
    public interface ITradeCreateHandler
    {
        Task OnBeforeTradeCreateAsync(ProductTrade trade);
        Task OnAfterTradeCreateAsync(ProductTrade trade);
    }

    public interface IOrderCreateHandler
    {
        Task OnBeforeOrderCreateAsync(ProductTradeOrder order);
        Task OnAfterOrderCreateAsync(ProductTradeOrder order);
    }

    public interface IPayTradeHandler
    {
        Task OnAfterFinishPayAsync(ProductTrade tradeSet);

        Task OnBeforeCreatePayAsync(ProductTrade trade);

        Task OnAfterClosePayAsync(ProductTrade trade);
    }

    public interface ITradeCancelHandler
    {
        Task<bool> OnTradeCancelAsync(ProductTrade trade);
    }

    public interface ITradeDeliveryHandler
    {
        Task<bool> OnTradeDeliveryAsync(ProductTrade trade);
    }

    public interface ITradeFinishHandler
    {
        Task<bool> OnTradeFinishAsync(ProductTrade trade);
    }
}
