using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Caching;

namespace ViazyNetCore.MultiTenancy
{
    public class FreeSqlTenantStore : ITenantStore
    {
        private readonly IFreeSql _freeSql;
        private readonly ICache _cache;

        public FreeSqlTenantStore(IFreeSql freeSql, ICache cache)
        {
            this._freeSql = freeSql;
            this._cache = cache;
        }

        private const string TENANT_CACHEKEY = "__CaesarTenantStore";

        public async Task<TenantConfiguration> FindAsync(int id)
        {
            return await this._cache.GetAsync(TENANT_CACHEKEY + id, () =>
                this._freeSql.GetRepository<TenantConfiguration>().Select.Where(p => p.Id == id).FirstAsync());
        }

        public async Task<TenantConfiguration> FindAsync(string name)
        {
            return await this._cache.GetAsync(TENANT_CACHEKEY + "_NAME_" + name, () =>
                this._freeSql.GetRepository<TenantConfiguration>().Select.Where(p => p.Name == name).FirstAsync());
        }

        public TenantConfiguration Find(int id)
        {
            return this._cache.Get(TENANT_CACHEKEY + id, () =>
                this._freeSql.GetRepository<TenantConfiguration>().Select.Where(p => p.Id == id).First());
        }

        public TenantConfiguration Find(string name)
        {
            return this._cache.Get(TENANT_CACHEKEY + "_NAME_" + name, () =>
                this._freeSql.GetRepository<TenantConfiguration>().Select.Where(p => p.Name == name).First());
        }
    }
}
