using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    public interface IProductHandler
    {
        Task<PageData<Product>> OnFindProductAllAsync(FindAllArguments args);
        Task OnGetProductAsync(ProductModel product);
        Task OnGetProductInfoAsync(ProductInfoModel productInfoModel);
    }

    public interface IEditProductHanlder
    {
        Task OnAddProductAsync(ProductManageModel product);
        Task OnUpdateProductAsync(ProductManageModel product);
        Task OnModifyProductStatusAsync(string productId, string shopId,ProductStatus status);
    }
}
