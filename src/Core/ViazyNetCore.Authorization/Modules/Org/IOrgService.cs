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
        Task<OrgGetOutput> GetAsync(long id);

        Task<List<OrgListOutput>> GetListAsync(string key);

        Task<long> AddAsync(OrgAddInput input);

        Task UpdateAsync(OrgUpdateInput input);

        Task DeleteAsync(long id);

        Task SoftDeleteAsync(long id);
    }
}
