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
    public class PrimssionKeyDataSeed : IPrimssionKeyDataSeed
    {
        private readonly IApiManager _apiManager;
        private readonly PermissionService _permissionService;

        public PrimssionKeyDataSeed(IApiManager apiManager, PermissionService permissionService)
        {
            this._apiManager = apiManager;
            this._permissionService = permissionService;
        }

        public async Task CreatePrimissionKeyAsync()
        {
            foreach (var primssion in this._apiManager.GetPermissionKeys())
            {
                await _permissionService.AddPermission(primssion, primssion, true);
            }
        }
    }
}
