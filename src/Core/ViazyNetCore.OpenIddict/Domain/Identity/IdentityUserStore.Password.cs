using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ViazyNetCore.OpenIddict.Domain
{
    public partial class IdentityUserStore : IUserPasswordStore<IdentityUser>
    {
        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
