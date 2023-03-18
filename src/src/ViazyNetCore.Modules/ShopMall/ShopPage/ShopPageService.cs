using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.ShopMall.Repositories;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class ShopPageService
    {
        private readonly IShopPageRepository _shopPageRepository;

        public ShopPageService(IShopPageRepository shopPageRepository)
        {
            this._shopPageRepository = shopPageRepository;
        }

        public Task<PageData<ShopPage>> GetPageList(ShopPageQuery query)
        {
            return this._shopPageRepository.GetPageList(query);
        }

        public Task EditPage(ShopPageEditModel model)
        {
            if (model.Id == 0)
            {
                throw new ApiException("无效请求");
            }
            return this._shopPageRepository.UpdateDiy
                .Where(p => p.Id == model.Id)
                .SetDto(model)
                .Set(p => p.ModifyTime == DateTime.Now)
                .ExecuteAffrowsAsync();
        }

        public Task AddPage(ShopPageCreateModel model)
        {
            var shopPage = model.CopyTo<ShopPage>();
            shopPage.CreateTime = DateTime.Now;
            shopPage.ModifyTime = DateTime.Now;
            return this._shopPageRepository.InsertAsync(shopPage);
        }

        public Task DeletePage(long id)
        {
            return this._shopPageRepository.UpdateDiy
                .Where(p => p.Id == id).Set(p => p.Status == ComStatus.Deleted)
                .ExecuteAffrowsAsync();
        }
    }
}
