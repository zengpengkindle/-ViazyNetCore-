using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ViazyNetCore.Modules.ShopMall.Repositories;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class ShopPageService
    {
        private readonly IShopPageRepository _shopPageRepository;
        private readonly IShopPageItemRepository _shopPageItemRepository;

        public ShopPageService(IShopPageRepository shopPageRepository, IShopPageItemRepository shopPageItemRepository)
        {
            this._shopPageRepository = shopPageRepository;
            this._shopPageItemRepository = shopPageItemRepository;
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

        public async Task UpdateDesginAsync(string code, List<DesginItem> items)
        {
            var page = await this._shopPageRepository.GetByCode(code);
            if (page == null)
                throw new ApiException("无效记录");
            await this._shopPageItemRepository.DeleteByCode(code);
            var list = new List<ShopPageItem>();
            var count = 0;
            items.ForEach(p =>
            {
                var model = new ShopPageItem
                {
                    WidgetCode = p.Type,
                    PageCode = code,
                    PositionId = count,
                    Sort = count + 1,
                    Parameters = p.Value.ToString()
                };
                list.Add(model);
                count++;
            });
            if (list.Any())
            {
                await this._shopPageItemRepository.InsertAsync(list);
            }
        }

        public async Task<ShopPage> GetPageById(long id)
        {
            return await this._shopPageRepository.GetAsync(id);
        }

        public async Task<List<DesginItem>> GetPageItems(string pagecode)
        {
            var pageItems = await this._shopPageItemRepository.Select.Where(p => p.PageCode == pagecode).ToListAsync();

            return pageItems.Select(p => new DesginItem
            {
                Type = p.WidgetCode,
                Value = JSON.Parse<JObject>(p.Parameters)
            }).ToList();
        }
    }
}
