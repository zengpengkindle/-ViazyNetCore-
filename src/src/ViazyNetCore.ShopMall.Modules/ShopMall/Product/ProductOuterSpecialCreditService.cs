using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class ProductOuterSpecialCreditService
    {
        private readonly ICacheService _cacheService;
        private readonly IFreeSql _engine;
        private readonly CreditService _creditService;

        public ProductOuterSpecialCreditService(ICacheService cacheService, IFreeSql engine, CreditService creditService)
        {
            this._cacheService = cacheService;
            this._engine = engine;
            this._creditService = creditService;
        }

        public async Task AddAsync(ProductOuterSpecialCredit productOuter)
        {
            var creditType = await this._creditService.GetCreditByCreditKey(productOuter.CreditKey);
            if (creditType.CreditType == CreditType.ReadyMoney && productOuter.ComputeType != ComputeType.Alone)
            {
                throw new ApiException("现金类型只支持独立定价");
            }
            var cacheKeyOuterKeySpecialCredit = GetCacheKey_OuterTypeSpecialCreditOfOuterType(productOuter.OuterType);
            this._cacheService.Remove(cacheKeyOuterKeySpecialCredit);

            await this._engine.Insert<ProductOuterSpecialCredit>().AppendData(productOuter).ExecuteAffrowsAsync();
            await GetOuterKeySpecialCreditOfOuterKey(productOuter.OuterType);

        }

        public Task<ProductOuterSpecialCredit> GetAsync(string id)
        {
            return this._engine.Select<ProductOuterSpecialCredit>().Where(p => p.Id == id).ToOneAsync();
        }

        public async Task ManageAsync(ProductOuterSpecialCredit specialCredit)
        {
            if (specialCredit.Id.IsNull())
                throw new ApiException("无效的修改");

            var oldSpecialCredit = await GetAsync(specialCredit.Id);
            if (oldSpecialCredit.OuterType != specialCredit.OuterType)
                throw new ApiException("外部关联键禁止修改");

            var cacheKeyOuterKeySpecialCredit = GetCacheKey_OuterTypeSpecialCreditOfOuterType(oldSpecialCredit.OuterType);
            this._cacheService.Remove(cacheKeyOuterKeySpecialCredit);

            await this._engine.Update<ProductOuterSpecialCredit>().Where(p => p.Id == specialCredit.Id).SetDto(new
            {
                //specialCredit.OuterType,
                specialCredit.CreditKey,
                specialCredit.ObjectName,
                specialCredit.ComputeType,
                specialCredit.FeeMoney,
                specialCredit.FeePercent,
                specialCredit.Exdata
            }).ExecuteAffrowsAsync();

            await GetOuterKeySpecialCreditOfOuterKey(oldSpecialCredit.OuterType);
        }

        public Task ModifyStatus(string outerId, ComStatus status)
        {
            return this._engine.Update<ProductOuterSpecialCredit>().SetDto(new
            {
                Id = outerId,
                Status = status
            }).ExecuteAffrowsAsync();
        }

        public Task<PageData<ProductOuterSpecialCredit>> FindAll(SpecialCreditPagination args)
        {
            return this._engine.Select<ProductOuterSpecialCredit>().WhereIf(args.OuterType.IsNotNull(), p => p.OuterType == args.OuterType)
                .OrderByDescending(p => p.CreateTime).ToPageAsync(args);
        }

        public Task<List<OuterKeySpecialCredit>> GetSpecialCreditByOuterKey(string outerType)
        {
            ComputeType[] computeTypes = new ComputeType[] {
                 ComputeType.Alone,
                  ComputeType.Hybrid,
                   ComputeType.Requirement,
                   ComputeType.Gift
            };
            return this._engine.Select<ProductOuterSpecialCredit>().Where(p => p.Status == ComStatus.Enabled && p.OuterType == outerType && computeTypes.Contains(p.ComputeType))
                   .OrderBy(p => p.CreditKey).WithTempQuery(p => new OuterKeySpecialCredit
                   {
                       Key = p.ObjectKey,
                       CreditKey = p.CreditKey,
                       Name = p.ObjectName,
                       ComputeType = p.ComputeType
                   }).ToListAsync();
        }

        public async Task<IList<OuterKeySpecialCredit>> GetOuterKeySpecialCreditOfOuterKey(string outerType)
        {
            var cacheKeyOuterKeySpecialCredit = GetCacheKey_OuterTypeSpecialCreditOfOuterType(outerType);
            var outerKeyCredits = this._cacheService.Get<IList<OuterKeySpecialCredit>>(cacheKeyOuterKeySpecialCredit);
            if (outerKeyCredits == null)
            {
                outerKeyCredits = await this._engine.Select<ProductOuterSpecialCredit>().Where(p => p.Status == ComStatus.Enabled && p.OuterType == outerType).WithTempQuery(p => new OuterKeySpecialCredit
                {
                    Key = p.ObjectKey,
                    CreditKey = p.CreditKey,
                    Name = p.ObjectName,
                    ComputeType = p.ComputeType,
                    FeeMoney = p.FeeMoney,
                    FeePercent = p.FeePercent
                }).ToListAsync();
                this._cacheService.Set(cacheKeyOuterKeySpecialCredit, outerKeyCredits, CachingExpirationType.ObjectCollection);
            }

            return outerKeyCredits;
        }

        private string GetCacheKey_OuterTypeSpecialCreditOfOuterType(string outerType)
        {
            return $"OuterTypeSpecialCredit:OuterType:{outerType}";
        }
    }
}
