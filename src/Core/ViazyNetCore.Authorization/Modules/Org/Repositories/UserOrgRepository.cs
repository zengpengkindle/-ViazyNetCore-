using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Repositories
{
    [Injection]
    public class UserOrgRepository : DefaultRepository<BmsUserOrg, long>, IUserOrgRepository
    {
        public UserOrgRepository(IFreeSql fsql) : base(fsql)
        {
        }

        /// <summary>
        /// 本部门下是否有员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> HasUser(long id)
        {
            return await Select.Where(a => a.OrgId == id).AnyAsync();
        }

        /// <summary>
        /// 部门列表下是否有员工
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<bool> HasUser(List<long> idList)
        {
            return await Select.Where(a => idList.Contains(a.OrgId)).AnyAsync();
        }
    }
}
