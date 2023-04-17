using ViazyNetCore.Model.ShopMall;

namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public class ProductSpecValueRepository : DefaultRepository<ProductSpecValue, string>, IProductSpecValueRepository
    {
        public ProductSpecValueRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public Task SetProductSpecValueStatus(string productId, ProductStatus status)
        {
            return this.UpdateDiy.Where(p => p.ProductId == productId).Set(p => p.Status == status).ExecuteAffrowsAsync();
        }

        public Task UpsertAsync(List<ProductSpecValue> shopsProductSpecValues)
        {
            return this.Orm.InsertOrUpdate<ProductSpecValue>().SetSource(shopsProductSpecValues).ExecuteAffrowsAsync();
        }
    }
}
