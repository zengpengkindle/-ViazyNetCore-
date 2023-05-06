using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Repositories
{
    [Injection]
    public interface IUserOrgRepository : IBaseRepository<BmsUserOrg, long>
    {
        /// <summary>
        /// 本部门下是否有员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> HasUser(long id);

        /// <summary>
        /// 部门列表下是否有员工
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<bool> HasUser(List<long> orgIdList);
    }
}
