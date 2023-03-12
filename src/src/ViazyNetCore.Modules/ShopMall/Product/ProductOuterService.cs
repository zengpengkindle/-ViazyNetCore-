using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class ProductOuterService
    {
        private readonly IFreeSql _engine;

        public ProductOuterService(IFreeSql engine)
        {
            this._engine = engine;
        }

        public Task AddProductOuter(ProductOuter productOuter)
        {
            return this._engine.Insert<ProductOuter>().AppendData(productOuter).ExecuteAffrowsAsync();
        }


        public Task<ProductOuter> GetAsync(string id)
        {
            return this._engine.Select<ProductOuter>().Where(p => p.Id == id).ToOneAsync();
        }

        public Task ManageProductOuter(ProductOuter productOuter)
        {
            if (productOuter.Id.IsNull())
                throw new ApiException("无效的修改");

            return this._engine.Update<ProductOuter>().Where(p => p.Id == productOuter.Id).SetDto(new
            {
                productOuter.OuterName,
                //productOuter.OuterType,
                productOuter.BeginTime,
                productOuter.EndTime,
                productOuter.Description
            }).ExecuteAffrowsAsync();
        }

        public Task ModifyStatus(string outerId, ComStatus status)
        {
            return this._engine.Update<ProductOuter>().SetDto(new
            {
                Id = outerId,
                Status = status
            }).ExecuteAffrowsAsync();
        }

        public Task<PageData<ProductOuter>> FindAll(Pagination args)
        {
            return this._engine.Select<ProductOuter>().OrderByDescending(p => p.CreateTime).ToPageAsync(args);
        }

        public async Task<Dictionary<string, string>> GetAllAsync()
        {
            var result = await this._engine.Select<ProductOuter>().Where(p => p.Status == ComStatus.Enabled).WithTempQuery(p => new { p.OuterType, p.OuterName }).ToListAsync();

            return result.ToDictionary(p => p.OuterType, p1 => p1.OuterName);
        }
    }
}
