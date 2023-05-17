using ViazyNetCore;

namespace ViazyNetCore.Modules.Repository
{
    /// <summary>
    /// 表示一个商品规格仓储
    /// </summary>
    [Injection]
    public class ProductSkuRepository : DefaultRepository<ProductSku, string>, IProductSkuRepository
    {
        /// <summary>
        /// 实例化一个<see cref="ProductSkuRepository"/>
        /// </summary>
        /// <param name="fsql"></param>
        public ProductSkuRepository(IFreeSql fsql) : base(fsql)
        {
        }

        ///<inheritdoc/>
        public Task<int> EditProductSpecCostPriceAsync(string productId, List<ProductSkuPriceDto> skuCostPrices)
        {

            var sources = skuCostPrices
                  .Select(p => new ProductSku()
                  {
                      ProductId = productId,
                      Id = p.SkuId,
                      Cost = p.CostPrice,
                  });
            return this.UpdateDiy.SetSource(sources).UpdateColumns(p => p.Cost).ExecuteAffrowsAsync();
        }

        ///<inheritdoc/>
        public Task<List<ProductSkuPriceDto>> GetProductSkuPriceAsync(string productId)
        {
            return this.Select.Where(p => p.ProductId == productId && p.Status == ComStatus.Enabled).ToListAsync(p => new ProductSkuPriceDto
            {
                SkuId = p.Id,
                Code = p.OuterSkuId,
                CostPrice = p.Cost,
                Price = p.Price,
                SpecDetailIdsStr = p.SpecDetailIds,
                SpecDetailText = p.SpecDetailText
            });
        }

        ///<inheritdoc/>
        public Task SetProductSkuStatus(string productId, ComStatus disabled)
        {
            return this.UpdateDiy.Where(p => p.ProductId == productId).Set(p => p.Status == disabled).ExecuteAffrowsAsync();
        }

        ///<inheritdoc/>
        public Task UpsertAsync(List<ProductSku> shopsProdcutSkus)
        {
            return this.Orm.InsertOrUpdate<ProductSku>().UpdateColumns(p => new
            {
                p.UpdateTime,
                p.OuterSkuId,
                p.Image,
                p.Price,
                p.SpecDetailIds,
                p.SpecDetailText,
                p.Status
            }).SetSource(shopsProdcutSkus).ExecuteAffrowsAsync();
        }
    }
}
