using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.CmsKit.Modules.Repositories;
using ViazyNetCore.Caching;

namespace ViazyNetCore.CmsKit.Modules
{
    [Injection]
    public class DictionaryService : IDictionaryService
    {
        private readonly IDictionaryTypeRepository _dictionaryTypeRepository;
        private readonly IDictionaryValueRepository _dictionaryValueRepository;
        private readonly ICacheService _cacheService;

        public DictionaryService(IDictionaryTypeRepository dictionaryTypeRepository
            , IDictionaryValueRepository dictionaryValueRepository
            , ICacheService cacheService)
        {
            this._dictionaryTypeRepository = dictionaryTypeRepository;
            this._dictionaryValueRepository = dictionaryValueRepository;
            this._cacheService = cacheService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <returns></returns>
        public async Task<PageData<DictionaryTypeListOutput>> GetPageAsync(DictionaryTypeFindAllArgs args)
        {
            var key = args.Name;

            var list = await this._dictionaryTypeRepository.Select
            .WhereIf(args.Name.IsNotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Sort)
            .Page(args.Page, args.Limit)
            .ToListAsync<DictionaryTypeListOutput>();

            var data = new PageData<DictionaryTypeListOutput>()
            {
                Rows = list,
                Total = total
            };

            return data;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(DictionaryTypeAddInput input)
        {
            var dictionaryType = input.CopyTo<DictionaryType>();
            await _dictionaryTypeRepository.InsertAsync(dictionaryType);
            return dictionaryType.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(DictionaryTypeUpdateInput input)
        {
            var entity = await _dictionaryTypeRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw new ApiException("数据字典不存在！");
            }

            input.CopyTo(entity);
            await _dictionaryTypeRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            //删除字典数据
            await _dictionaryValueRepository.DeleteAsync(a => a.DictionaryTypeId == id);

            //删除数据字典类型
            await _dictionaryTypeRepository.DeleteAsync(a => a.Id == id);
        }

        public Task<DictionaryType> GetAsync(long id)
        {
            return this._dictionaryTypeRepository.GetAsync(id);
        }

        public Task<DictionaryValue> GetValueAsync(long id)
        {
            return this._dictionaryValueRepository.GetAsync(id);
        }

        public async Task<PageData<DictionaryValue>> GetValuePageAsync(DictionaryValueFindAllArgs args)
        {
            var key = args.Name;

            var result = await this._dictionaryValueRepository.Select
            .Where(p => p.DictionaryTypeId == args.DictionaryTypeId)
            .WhereIf(args.Name.IsNotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .OrderByDescending(true, c => c.Sort)
            .ToPageAsync(args);
            return result;
        }

        public Task DeleteValueAsync(long id)
        {
            //删除字典数据
            return this._dictionaryValueRepository.DeleteAsync(a => a.Id == id);
        }

        public async Task<long> AddValueAsync(DictionaryValueAddInput input)
        {
            var type = await this._dictionaryTypeRepository.GetAsync(input.DictionaryTypeId);
            if (type == null)
            {
                throw new ApiException("数据字典不存在！");
            }

            var dictionaryValue = input.CopyTo<DictionaryValue>();
            dictionaryValue.CreateTime = DateTime.Now;
            await _dictionaryValueRepository.InsertAsync(dictionaryValue);

            var cacheKey = this.GetCacheKey_DicionaryValueFromCode(type.Code);
            this._cacheService.Remove(cacheKey);

            return dictionaryValue.Id;
        }

        public async Task UpdateValueAsync(DictionaryValueUpdateInput input)
        {
            var type = await this._dictionaryTypeRepository.GetAsync(input.DictionaryTypeId);
            if (type == null)
            {
                throw new ApiException("数据字典不存在！");
            }
            var entity = await _dictionaryValueRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw new ApiException("数据字典不存在！");
            }

            input.CopyTo(entity);
            await _dictionaryValueRepository.UpdateAsync(entity);

            var cacheKey = this.GetCacheKey_DicionaryValueFromCode(type.Code);
            this._cacheService.Remove(cacheKey);
        }

        public async Task<List<DictionaryValue>> GetAllValuesAsync(string code)
        {
            var cacheKey = this.GetCacheKey_DicionaryValueFromCode(code);
            return await this._cacheService.GetAsync(cacheKey, async () =>
                {
                    var type = await this._dictionaryTypeRepository.Where(a => a.Code == code).FirstAsync();
                    if (type == null)
                        return new List<DictionaryValue>();
                    var result = await this._dictionaryValueRepository.Select
                    .Where(p => p.DictionaryTypeId == type.Id)
                    .OrderByDescending(true, c => c.Sort)
                    .ToListAsync();
                    return result;
                }, CachingExpirationType.ObjectCollection);
        }

        private string GetCacheKey_DicionaryValueFromCode(string code)
        {
            return $"Dicionary:Values:Code:{code}";
        }
    }
}
