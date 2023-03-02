using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.Authorization.Controllers
{
    public class OrgController : DynamicControllerBase
    {
        private readonly IOrgService _orgService;

        public OrgController(IOrgService orgService)
        {
            this._orgService = orgService;
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public Task<OrgGetOutput> GetAsync(long id)
        {
            return _orgService.GetAsync(id);
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public Task<List<OrgListOutput>> GetListAsync(string key)
        {
            return _orgService.GetListAsync(key);
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public Task<long> AddAsync(OrgAddInput input)
        {
            return _orgService.AddAsync(input);
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public async Task<bool> UpdateAsync(OrgUpdateInput input)
        {
            await _orgService.UpdateAsync(input);
            return true;
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public async Task<bool> DeleteAsync(long id)
        {
            await _orgService.DeleteAsync(id);
            return true;
        }

        [HttpPost]
        [Permission(PermissionIds.User)]
        public async Task<bool> SoftDeleteAsync(long id)
        {
            await _orgService.SoftDeleteAsync(id);
            return true;
        }
    }
}
