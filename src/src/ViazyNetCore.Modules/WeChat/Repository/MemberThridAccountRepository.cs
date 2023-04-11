using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public class MemberThridAccountRepository : DefaultRepository<MemberThridAccount, long>, IMemberThridAccountRepository
    {
        public MemberThridAccountRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
