using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ViazyNetCore.Identity.Domain
{
    public partial class IdentityUserStore : IUserRoleStore<IdentityUser>
    {
        public Task AddToRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Check.NotNull(user, nameof(user));
            await SetRoleByUserId(user);
            return user.Roles.Select(p => p.ToString()).ToList();
        }

        public Task<IList<IdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Check.NotNull(user, nameof(user));

            var role = await this._roleService.GetByNameAsync(roleName);
            return await this._roleService.IsUserInRoles(user.Id, role.Id);
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected virtual async Task SetRoleByUserId(IdentityUser identity)
        {
            if (identity.Roles.IsNullOrEmpty())
            {
                var roles = await this._roleService.GetRoleIdsOfUserByCache(identity.Id);
                identity.Roles = roles;
            }
        }
    }
}
