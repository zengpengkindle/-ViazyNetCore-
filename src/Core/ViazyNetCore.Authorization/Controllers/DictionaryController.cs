using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Authorization.ViewModels;

namespace ViazyNetCore.Authorization
{
    public class DictionaryController : DynamicControllerBase
    {
        private readonly IDictionaryService _dictionaryService;
        private readonly IMapper _mapper;

        public DictionaryController(IDictionaryService dictionaryService, IMapper mapper)
        {
            this._dictionaryService = dictionaryService;
            this._mapper = mapper;
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

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public async Task<PageData<DictionaryValue>> FindValuesAsync(DictionaryValueFindAllArgs args)
        {
            return await this._dictionaryService.GetValuePageAsync(args);
        }

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        [Route("getvalues")]
        public async Task<List<DictionaryValueViewResult>> FindAllValuesAsync(string code)
        {
            var result = await this._dictionaryService.GetAllValuesAsync(code);
            return this._mapper.Map<List<DictionaryValue>, List<DictionaryValueViewResult>>(result);
        }

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public Task<long> AddValueAsync(DictionaryValueAddInput input)
        {
            return this._dictionaryService.AddValueAsync(input);
        }

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public async Task<bool> UpdateValueAsync(DictionaryValueUpdateInput input)
        {
            await this._dictionaryService.UpdateValueAsync(input);
            return true;
        }

        [HttpPost]
        [Permission(PermissionIds.Setting)]
        public async Task<bool> DeleteValueAsync(long id)
        {
            await this._dictionaryService.DeleteValueAsync(id);
            return true;
        }
    }
}
