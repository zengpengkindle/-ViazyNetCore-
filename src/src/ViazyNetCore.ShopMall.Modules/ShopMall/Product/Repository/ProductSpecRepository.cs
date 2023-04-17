using ViazyNetCore.Model.ShopMall;

namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public class ProductSpecRepository : DefaultRepository<ProductSpec, string>, IProductSpecRepository
    {
        public ProductSpecRepository(IFreeSql fsql) : base(fsql)
        {
        }


        public Task<List<ProductSpecTreeDto>> GetProductSpecTreeAsync(string productId)
        {
            return Task.FromResult(this.Select.Where(p => p.ProductId == productId && p.Status == ComStatus.Enabled).OrderBy(p => p.Sort).ToList(s => new ProductSpecTreeDto
            {
                Key = s.Id,
                Text = s.Name,
                Value = this.Orm.Select<ProductSpecValue>().Where(sv => sv.ProductId == s.Id && sv.Status == ProductStatus.OnSale).ToList(b => new ProductSkuDto
                {
                    Key = b.Id,
                    Text = b.Name
                }),
            }));
        }

        public Task SetProductSpecStatus(string productId, ComStatus disabled)
        {
            return this.UpdateDiy.Where(p => p.ProductId == productId).Set(p => p.Status == disabled).ExecuteAffrowsAsync();
        }

        public Task UpsertAsync(List<ProductSpec> shopsProductSpecs)
        {
            return this.Orm.InsertOrUpdate<ProductSpec>().SetSource(shopsProductSpecs).ExecuteAffrowsAsync();
        }
    }
}
