using ChainGo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Modules.Repositories
{
    public interface IUserRoleRepository
    {
        Task AddUserToRoles(string userId, List<string> roleIds);
    }
}
