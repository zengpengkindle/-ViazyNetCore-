using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.Cloud.Abstract;
using Microsoft.Extensions.Options;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Data.FreeSql;
using ViazyNetCore.Filter.Descriptor;

namespace ViazyNetCore.Authorization
{
    public class PrimssionKeyDataSeed
    {
        private readonly IApiManager _apiManager;
        private readonly IFreeSql _freeSql;
        private readonly IOptions<DbConfig> _options;
        private readonly PermissionService _permissionService;

        public PrimssionKeyDataSeed(IApiManager apiManager, IFreeSql freeSql, IOptions<DbConfig> options, PermissionService permissionService)
        {
            this._apiManager = apiManager;
            this._freeSql = freeSql;
            this._options = options;
            this._permissionService = permissionService;
        }

        public async Task CreatePrimissionKeyAsync()
        {
            var fsql = this._freeSql.UseDb(this._options.Value.Key);
            foreach (var primssion in this._apiManager.GetPermissionKeys())
            {
                await _permissionService.AddPermission(primssion, primssion, true);
            }
        }
    }
}
