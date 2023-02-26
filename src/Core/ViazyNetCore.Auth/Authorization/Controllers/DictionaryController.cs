using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Authorization.Modules;

namespace ViazyNetCore.Authorization
{
    public class DictionaryController : DynamicControllerBase
    {
        private readonly IDictionaryService _dictionaryService;

        public DictionaryController(IDictionaryService dictionaryService)
        {
            this._dictionaryService = dictionaryService;
        }

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public Task<PageData<DictionaryTypeListOutput>> FindAllAsync(DictionaryTypeFindAllArgs args)
        {
            return this._dictionaryService.GetPageAsync(args);
        }

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public Task<long> AddAsync(DictionaryTypeAddInput input)
        {
            return this._dictionaryService.AddAsync(input);
        }

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public async Task<bool> UpdateAsync(DictionaryTypeUpdateInput input)
        {
            await this._dictionaryService.UpdateAsync(input);
            return true;
        }

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public async Task<bool> DeleteAsync(long id)
        {
            await this._dictionaryService.DeleteAsync(id);
            return true;
        }


        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public async Task<DictionaryType> GetAsync(long id)
        {
            return await this._dictionaryService.GetAsync(id);
        }

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public async Task<DictionaryValue> GetValueAsync(long id)
        {
            return await this._dictionaryService.GetValueAsync(id);
        }
    }
}
