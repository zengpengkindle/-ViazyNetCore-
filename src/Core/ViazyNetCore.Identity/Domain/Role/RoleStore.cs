using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Data.FreeSql.Extensions;
using ViazyNetCore.Identity.Domain.Models;

namespace ViazyNetCore.Identity.Domain
{
    public class RoleStore : IRoleStore<BmsRole>
    {
        private readonly RoleService _roleService;

        public RoleStore(RoleService roleService)
        {
            this._roleService = roleService;
        }

        public async Task<IdentityResult> CreateAsync(BmsRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));
            await this._roleService.UpdateAsync(role);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(BmsRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));
            await this._roleService.DeleteRoleAsync(role.Id);

            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<BmsRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await this._roleService.GetAsync(roleId.ParseTo<long>());
        }

        public async Task<BmsRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await this._roleService.GetByNameAsync(normalizedRoleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(BmsRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task<string> GetRoleIdAsync(BmsRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(BmsRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(BmsRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(BmsRole role, string roleName, CancellationToken cancellationToken)
        {
            Check.NotNull(role, nameof(role));
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(BmsRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));
            await this._roleService.UpdateAsync(role);

            return IdentityResult.Success;
        }
    }
}
