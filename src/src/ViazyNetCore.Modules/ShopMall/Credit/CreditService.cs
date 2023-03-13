using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class CreditService
    {
        private readonly ICacheService _cacheService;
        private readonly IFreeSql _engine;

        public CreditService(ICacheService cacheService, IFreeSql engine)
        {
            this._cacheService = cacheService;
            this._engine = engine;
        }

        public async Task CreateCredit(string name, string key, CreditType creditType, string Exdata = null)
        {
            if (await this._engine.Select<Credits>().AnyAsync(p => p.CreditKey == key))
            {
                throw new ApiException("货币标识重复");
            }
            var cacheKeyCredit = GetCacheKey_GetAllCredits();
            this._cacheService.Remove(cacheKeyCredit);
            await this._engine.Insert<Credits>().AppendData(new Credits
            {
                Id = Snowflake<Credits>.NextIdString(),
                CreditKey = key,
                Name = name,
                CreditType = creditType,
                Exdata = Exdata,
                Status = ComStatus.Enabled,
                CreateTime = DateTime.Now
            }).ExecuteAffrowsAsync();

            await GetAllCredits();
        }

        public async Task<Dictionary<string, string>> GetAllAsync()
        {
            var result = await this._engine.Select<Credits>().Where(p => p.Status == ComStatus.Enabled).WithTempQuery(p => new { p.CreditKey, p.Name }).ToListAsync();

            return result.ToDictionary(p => p.CreditKey, p1 => p1.Name);
        }

        public Task ModifyCreditStatus(string creditId, ComStatus status)
        {
            var cacheKeyCredit = GetCacheKey_GetAllCredits();
            this._cacheService.Remove(cacheKeyCredit);
            return this._engine.Update<Credits>().SetDto(new
            {
                Id = creditId,
                Status = status
            }).ExecuteAffrowsAsync();
        }

        public Task<PageData<Credits>> FindAll(Pagination args)
        {
            return this._engine.Select<Credits>().OrderByDescending(p => p.CreateTime).ToPageAsync(args);
        }

        public async Task<IList<Credits>> GetAllCredits()
        {
            var cacheKeyCredit = GetCacheKey_GetAllCredits();
            var credits = this._cacheService.Get<IList<Credits>>(cacheKeyCredit);
            if (credits == null)
            {
                credits = await this._engine.Select<Credits>().ToListAsync();
                this._cacheService.Set(cacheKeyCredit, credits, CachingExpirationType.ObjectCollection);
            }

            return credits;
        }

        public async Task<Credits> GetCreditByCreditKey(string creditKey)
        {
            IEnumerable<Credits> userRoleIds = await GetAllCredits();
            return userRoleIds.Where(p => p.CreditKey == creditKey).SingleOrDefault();
        }

        private string GetCacheKey_GetAllCredits()
        {
            return $"AllCredits";
        }
    }
}
