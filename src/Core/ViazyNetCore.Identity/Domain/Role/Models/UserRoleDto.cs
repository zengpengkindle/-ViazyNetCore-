using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Identity.Domain.Models
{
    public class UserRoleDto
    {
        public long UserId { get; set; }

        public List<long> RoleIds { get; set; }
    }
}
