using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Identity.Domain
{
    public class RoleManager : RoleManager<BmsRole>
    {
        public RoleManager(IRoleStore<BmsRole> store
            , IEnumerable<IRoleValidator<BmsRole>> roleValidators
            , ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors
            , ILogger<RoleManager<BmsRole>> logger) 
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
