using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.ShopMall.Models;

namespace ViazyNetCore.Modules.ShopMall
{
    public interface IProductHandler
    {
        Task<IResult<PageData<Product>>> OnFindProductAllAsync(FindAllArguments args);
        Task OnGetProductAsync(ProductModel product);
        Task OnGetProductInfoAsync(ProductInfoModel productInfoModel);
    }

    public interface IEditProductHanlder
    {
        Task<IResult> OnAddProductAsync(ProductModel product);
        Task<IResult> OnUpdateProductAsync(ProductModel product);
        Task<IResult> OnModifyProductStatusAsync(string productId, string shopId,ProductStatus status);
    }
}
