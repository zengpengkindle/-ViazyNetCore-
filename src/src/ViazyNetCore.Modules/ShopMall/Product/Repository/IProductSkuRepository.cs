using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Repository
{
    /// <summary>
    /// 表示一个商品规格仓储接口
    /// </summary>
    [Injection]
    public interface IProductSkuRepository : IBaseRepository<ProductSku, string>
    {
        /// <summary>
        /// 编辑商品规格成本价
        /// </summary>
        Task<int> EditProductSpecCostPriceAsync(string productId, List<ProductSkuPriceDto> skuCostPrices);
        /// <summary>
        /// 获取商品规格Sku信息
        /// </summary>
        Task<List<ProductSkuPriceDto>> GetProductSkuPriceAsync(string productId);
        /// <summary>
        /// 修改商品规格
        /// </summary>
        Task SetProductSkuStatus(string productId, ComStatus disabled);
        /// <summary>
        /// 更新商品规格
        /// </summary>
        /// <param name="shopsProdcutSkus"></param>
        /// <returns></returns>
        Task UpsertAsync(List<ProductSku> shopsProdcutSkus);
    }
}
