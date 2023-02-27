using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Authorization.Modules.Repositories;

namespace ViazyNetCore.Authorization.Modules
{
    [Injection]
    public class DictionaryService : IDictionaryService
    {
        private readonly IDictionaryTypeRepository _dictionaryTypeRepository;
        private readonly IDictionaryValueRepository _dictionaryValueRepository;

        public DictionaryService(IDictionaryTypeRepository dictionaryTypeRepository, IDictionaryValueRepository dictionaryValueRepository)
        {
            this._dictionaryTypeRepository = dictionaryTypeRepository;
            this._dictionaryValueRepository = dictionaryValueRepository;
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
            .OrderByDescending(true, c => c.Id)
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
    }
}
