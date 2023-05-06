using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization
{
    [Injection]
    public interface IOrgService
    {
        Task<OrgGetDto> GetAsync(long id);

        Task<List<OrgListDto>> GetListAsync(string key);

        Task<long> AddAsync(OrgAddDto input);

        Task UpdateAsync(OrgUpdateDto input);

        Task DeleteAsync(long id);

        Task SoftDeleteAsync(long id);
    }
}
