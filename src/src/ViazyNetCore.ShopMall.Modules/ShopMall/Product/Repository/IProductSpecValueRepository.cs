using ViazyNetCore.Model.ShopMall;

namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public interface IProductSpecValueRepository : IBaseRepository<ProductSpecValue, string>
    {
        Task SetProductSpecValueStatus(string productId, ProductStatus status);
        Task UpsertAsync(List<ProductSpecValue> shopsProductSpecValues);
    }
}
