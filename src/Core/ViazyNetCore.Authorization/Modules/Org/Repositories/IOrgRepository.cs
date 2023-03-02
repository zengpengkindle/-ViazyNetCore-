using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Repositories
{
    [Injection]
    public interface IOrgRepository : IBaseRepository<BmsOrg, long>
    {
        /// <summary>
        /// 获得本部门和下级部门Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<long>> GetChildIdListAsync(long id);
    }
}
