using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Modules
{
    [Injection]
    public interface IDictionaryService
    {
        Task<PageData<DictionaryTypeListOutput>> GetPageAsync(DictionaryTypeFindAllArgs args);

        Task<long> AddAsync(DictionaryTypeAddInput input);

        Task UpdateAsync(DictionaryTypeUpdateInput input);

        Task DeleteAsync(long id);
        Task<DictionaryValue> GetValueAsync(long id);
        Task<DictionaryType> GetAsync(long id);
        Task<PageData<DictionaryValue>> GetValuePageAsync(DictionaryValueFindAllArgs args);
        Task DeleteValueAsync(long id);
        Task<long> AddValueAsync(DictionaryValueAddInput input);
        Task UpdateValueAsync(DictionaryValueUpdateInput input);
        Task<List<DictionaryValue>> GetAllValuesAsync(string code);
    }
}
