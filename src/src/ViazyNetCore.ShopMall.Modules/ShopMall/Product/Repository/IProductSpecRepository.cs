namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public interface IProductSpecRepository : IBaseRepository<ProductSpec, string>
    {
        Task<List<ProductSpecTreeDto>> GetProductSpecTreeAsync(string productId);
        Task SetProductSpecStatus(string productId, ComStatus disabled);
        Task UpsertAsync(List<ProductSpec> shopsProductSpecs);
    }
}
